using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TGis.Common;
using System.Diagnostics;

namespace TGis.RemoteService
{
    class SessionStateChangeArgs
    {
        public enum Reason
        {
            Add,
            Remove,
        }
        public string SessionStr;
        public object SessionBody;
        public Reason SessionReason;
    }
    delegate void SessionStateChangeHandler(object sender, SessionStateChangeArgs args);
    class SessionMgr
    {
        private IDictionary<string, object> dictSession = new Dictionary<string, object>();
        private IDictionary<string, DateTime> dictSessionTime = new Dictionary<string, DateTime>();
        private TimeSpan timeout;
        private System.Timers.Timer timer = new System.Timers.Timer(2000);

        public SessionMgr(TimeSpan timeout)
        {
            this.timeout = timeout;
            timer.AutoReset = true;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(TimerRoutine);
            timer.Start();
        }
        public string Register(object obj, bool bLocked = false)
        {
            Guid guid = Guid.NewGuid();
            string ss = guid.ToString() + DateTime.Now.ToString();
            if (!bLocked)
                Lock();
            dictSession[ss] = obj;
            dictSessionTime[ss] = DateTime.Now;
            DispatchSessionStateChangeMsg(ss, obj, SessionStateChangeArgs.Reason.Add);
            if (!bLocked)
                Unlock();
            return ss;
        }
        public bool Unregister(string sessionString, bool bLocked = false)
        {
            if (!bLocked)
                Lock();
            object obj;
            bool br = dictSession.TryGetValue(sessionString, out obj);
            dictSession.Remove(sessionString);
            dictSessionTime.Remove(sessionString);
            if(br)
                DispatchSessionStateChangeMsg(sessionString, obj, SessionStateChangeArgs.Reason.Remove);
            if (!bLocked)
                Unlock();
            return br;
        }
        public bool Tick(string sessionString, bool bLocked = false)
        {
            if (!bLocked)
                Lock();
            DateTime tm;
            bool br = dictSessionTime.TryGetValue(sessionString, out tm);
            if (br)
            {
                dictSessionTime[sessionString] = DateTime.Now;
            }
            if (!bLocked)
                Unlock();
            return br;
        }
        public void Lock()
        {
            Monitor.Enter(this);
        }
        public void Unlock()
        {
            Monitor.Exit(this);
        }
        public void Stop()
        {
            timer.Stop();
        }
        public IDictionary<string, object> DictSession
        {
            get { return dictSession; }
        }
        public event SessionStateChangeHandler OnSessionStateChanged;

        private void TimerRoutine(object sender, System.Timers.ElapsedEventArgs e)
        {
            DateTime current = DateTime.Now;
            IList<string> expiredSessions = new List<string>();
            Lock();
            foreach (KeyValuePair<string, DateTime> kvSessionTime in dictSessionTime)
            {
                if ((current < kvSessionTime.Value)
                    || (current - kvSessionTime.Value > timeout))
                {
                    expiredSessions.Add(kvSessionTime.Key);
                }
            }
            foreach (string sessionString in expiredSessions)
                Unregister(sessionString);
            Unlock();
        }
        private void DispatchSessionStateChangeMsg(string ss, object sb, SessionStateChangeArgs.Reason reason)
        {
            if(OnSessionStateChanged == null) return;
            SessionStateChangeArgs arg = new SessionStateChangeArgs();
            arg.SessionReason = reason;
            arg.SessionStr = ss;
            arg.SessionBody = sb;
            OnSessionStateChanged(this, arg);
        }
    }
    class CarSession
    {
        public Car CarInstance;
        public double X;
        public double Y;
        public bool Alive;
        public CarRollDirection RollDirection;
        public DateTime LastUpdateTime;
        public string SessionStr;
        public bool OutOfPath;
        public bool ExceptionState
        {
            get { return (OutOfPath || (RollDirection != CarRollDirection.Forward)); }
        }
    }
    class CarSessionStateChangeArgs
    {
        public enum Reason
        {
            Add,
            Update,
            Remove,
            UpdateTemprary,
            Connect,
            Disconnect,
        };
        public CarSession CarSessionArg;
        public Reason ReasonArg;
    }
    delegate void CarSessionStateChangeHandler(object sender, CarSessionStateChangeArgs args);
    class CarSessionMgr
    {
        public delegate void EnumCarSessionHandler(CarSession cs);
        private SessionMgr sessionMgr = new SessionMgr(new TimeSpan(0, 5, 0));
        private CarMgr carMgr;
        private PathMgr pathMgr;
        private IDictionary<int, CarSession> dictCarSession = new Dictionary<int, CarSession>();
        private ICarTerminalAbility terminal;

        public CarSessionMgr(CarMgr cm, PathMgr pm)
        {
            carMgr = cm;
            pathMgr = pm;
            foreach (Car c in carMgr.Cars)
            {
                AddCarSession(c);
            }
            sessionMgr.OnSessionStateChanged += new SessionStateChangeHandler(SessionMsgHandler);
            carMgr.OnCarStateChanged += new CarStateChangeHandler(CarMgrHandler);
        }
        public ICarTerminalAbility Terminal
        {
            get { return terminal; }
            set
            {
                lock (this)
                {
                    if (terminal != null)
                        terminal.OnCarStateChanged -= new CarTerminalStateChangeHandler(TerminalCarHandler);
                    terminal = value;
                    if (terminal != null)
                        terminal.OnCarStateChanged += new CarTerminalStateChangeHandler(TerminalCarHandler);
                }
            }
        }

        public event CarSessionStateChangeHandler OnCarSessionStateChanged;
        public void Stop()
        {
            sessionMgr.Stop();
            sessionMgr.OnSessionStateChanged -= new SessionStateChangeHandler(SessionMsgHandler);
            carMgr.OnCarStateChanged -= new CarStateChangeHandler(CarMgrHandler);
        }
        public void EnumCarSession(EnumCarSessionHandler handler)
        {
            lock (this)
            {
                foreach (CarSession cs in dictCarSession.Values)
                    handler(cs);
            }
        }
        private void AddCarSession(Car c)
        {
            lock (this)
            {
                CarSession cs = new CarSession();
                cs.CarInstance = c;
                cs.Alive = false;
                cs.LastUpdateTime = DateTime.MinValue;
                cs.OutOfPath = false;
                dictCarSession[c.Id] = cs;
                DispatchSessionStateChangeMsg(cs, CarSessionStateChangeArgs.Reason.Add);
            }
        }
        private bool RemoveCarSession(Car c)
        {
            bool br;
            lock (this)
            {
                CarSession cs;
                br = dictCarSession.TryGetValue(c.Id, out cs);
                dictCarSession.Remove(c.Id);
                if(br)
                    DispatchSessionStateChangeMsg(cs, CarSessionStateChangeArgs.Reason.Remove);
            }
            return br;
        }
        private bool UpdateCarSession(Car c)
        {
            bool br;
            lock (this)
            {
                CarSession cs;
                br = dictCarSession.TryGetValue(c.Id, out cs);
                if (!br) return br;
                cs.CarInstance.Id = c.Id;
                cs.CarInstance.Name = c.Name;
                cs.CarInstance.PathId = c.PathId;
                DispatchSessionStateChangeMsg(cs, CarSessionStateChangeArgs.Reason.Update);
            }
            return br;
        }
        public bool TryGetCarSession(int id, out CarSession cs)
        {
            lock(this)
            {
                return dictCarSession.TryGetValue(id, out cs);
            }
        }
        private CarProcResult TerminalCarHandler(object sender, CarTernimalStateArg arg)
        {
            CarSession cs;
            int cid;
            if (!carMgr.MapSerialNumToCarId(arg.PhoneNum, out cid))
                return CarProcResult.Miss;
            lock (this)
            {
                if (!TryGetCarSession(cid, out cs))
                    return CarProcResult.Miss;
                cs.LastUpdateTime = arg.Time;
                if ((cs.SessionStr == null) || !sessionMgr.Tick(cs.SessionStr))
                {
                    Debug.Assert(cs.Alive == false);
                    cs.SessionStr = sessionMgr.Register(cs);
                    cs.Alive = true;
                    DispatchSessionStateChangeMsg(cs, CarSessionStateChangeArgs.Reason.Connect);
                }
                Debug.Assert(cs.Alive == true);
                cs.X = arg.X;
                cs.Y = arg.Y;
                cs.RollDirection = arg.RollDirection;
                cs.LastUpdateTime = arg.Time;
                cs.OutOfPath = false;
                Path p;
                if (pathMgr.TryGetPath(cs.CarInstance.PathId, out p))
                {
                    if (!p.PathPolygon.IsPointInRegion(new double[] { cs.X, cs.Y }))
                        cs.OutOfPath = true;
                    else
                        cs.OutOfPath = false;
                }
                DispatchSessionStateChangeMsg(cs, CarSessionStateChangeArgs.Reason.UpdateTemprary);
            }
            return CarProcResult.Ok;

        }
        private void SessionMsgHandler(object sender, SessionStateChangeArgs arg)
        {
            if (arg.SessionReason != SessionStateChangeArgs.Reason.Remove)
                return;
            lock (this)
            {
                CarSession cs = arg.SessionBody as CarSession;
                if (cs == null) return;
                if (!TryGetCarSession(cs.CarInstance.Id, out cs)) return;
                Debug.Assert(cs.Alive == true);
                cs.Alive = false;
                DispatchSessionStateChangeMsg(cs, CarSessionStateChangeArgs.Reason.Disconnect);
            }
        }
        private void CarMgrHandler(object sender, CarStateChangeArgs arg)
        {
            switch (arg.ReasonArg)
            {
                case CarStateChangeArgs.Reason.Add:
                    AddCarSession(arg.CarArg);
                    break;
                case CarStateChangeArgs.Reason.Remove:
                    RemoveCarSession(arg.CarArg);
                    break;
                case CarStateChangeArgs.Reason.Update:
                    UpdateCarSession(arg.CarArg);
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
        }
        private void DispatchSessionStateChangeMsg(CarSession cs, CarSessionStateChangeArgs.Reason reason)
        {
            CarSessionStateChangeArgs arg = new CarSessionStateChangeArgs();
            arg.CarSessionArg = cs;
            arg.ReasonArg = reason;
            if (OnCarSessionStateChanged != null)
                OnCarSessionStateChanged(this, arg);
        }
    }
}
