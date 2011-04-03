using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TGis.Viewer
{
    enum GisCarModelReasonCar
    {
        Add,
        Remove,
        Update,
    }
    enum GisCarModelReasonPath
    {
        Add,
        Remove,
    }
    delegate void CarShowableMsgHandler(object sender, GisCarModelReasonCar reason, int id, string name, double x,
        double y, bool bException);
    delegate void PathShowableMsgHandler(object sender, GisCarModelReasonPath reason, int id);
    class GisCarModel
    {
        class CarState
        {
            public bool UserMakeShowable = false;
            public bool Connected = false;
            public CarState()
            {

            }
            public CarState(bool bUserShow, bool bConnect)
            {
                UserMakeShowable = bUserShow;
                Connected = bConnect;
            }
            public bool ShowAble
            {
                get { return (UserMakeShowable && Connected); }
            }
        }
        class PathState
        {
            public bool Show;
        }
        private CarSessionMgr csm;

        internal CarSessionMgr CsMgr
        {
            get { return csm; }
        }
        private IDictionary<int, CarState> dictCarState = new Dictionary<int, CarState>();
        private IDictionary<int, PathState> dictPathState = new Dictionary<int, PathState>();
        public GisCarModel(CarSessionMgr csm)
        {
            this.csm = csm;
        }
        public void InitCarStateFirstTime()
        {
            lock (this)
            {
                csm.EnumCarSession(EnumCarSessionHandler);
                csm.OnCarSessionStateChanged += new CarSessionStateChangeHandler(CarSessionMsgHander);
            }
        }
        public void Stop()
        {
            csm.OnCarSessionStateChanged -= new CarSessionStateChangeHandler(CarSessionMsgHander);
        }
        public void UserMakeCarShowable(int id, bool bShow)
        {
            CarSession cs;
            if (!csm.TryGetCarSession(id, out cs))
                return;
            lock (this)
            {
                CarState carState;
                if (!dictCarState.TryGetValue(id, out carState))
                    return;

                OnCarAllStateUpdate(cs, bShow);
            }
        }
        public void UserMakePathShowable(int id, bool bShow)
        {
            lock (this)
            {
                PathState pathState;
                if (!dictPathState.TryGetValue(id, out pathState))
                {
                    pathState = new PathState();
                    pathState.Show = false;
                    dictPathState[id] = pathState;
                }
                bool bOld = pathState.Show;
                pathState.Show = bShow;
                if((bOld != pathState.Show)
                    && (OnPathShowableChanged != null))
                    OnPathShowableChanged(this, 
                        pathState.Show ? GisCarModelReasonPath.Add : GisCarModelReasonPath.Remove, id);
            }
        }
        public event CarShowableMsgHandler OnCarShowableChanged;
        public event PathShowableMsgHandler OnPathShowableChanged;

        private void CarSessionMsgHander(object sender, CarSessionStateChangeArgs arg)
        {
            lock (this)
            {
                switch (arg.ReasonArg)
                {
                    case CarSessionStateChangeArgs.Reason.Connect:
                        OnCarConnect(arg.CarSessionArg);
                        break;
                    case CarSessionStateChangeArgs.Reason.Disconnect:
                        OnCarDisconnect(arg.CarSessionArg);
                        break;
                    case CarSessionStateChangeArgs.Reason.UpdateTemprary:
                        OnCarLocationUpdate(arg.CarSessionArg);
                        break;
                }
            }
        }
        private void EnumCarSessionHandler(CarSession cs)
        {
            OnCarConnect(cs);
        }
        private void OnCarConnect(CarSession cs)
        {
            CarState carState;
            if (!dictCarState.TryGetValue(cs.CarInstance.Id, out carState))
            {
                carState = new CarState();
                dictCarState[cs.CarInstance.Id] = carState;
            }
            OnCarUpdate(cs);
        }
        private void OnCarDisconnect(CarSession cs)
        {
            CarState carState;
            if (!dictCarState.TryGetValue(cs.CarInstance.Id, out carState))
                return;
            OnCarUpdate(cs);
            dictCarState.Remove(cs.CarInstance.Id);
        }
        private void OnCarLocationUpdate(CarSession cs)
        {
            CarState carState;
            if (!dictCarState.TryGetValue(cs.CarInstance.Id, out carState))
                return;
            if (!carState.ShowAble)
                return;
            if (OnCarShowableChanged != null)
                OnCarShowableChanged(this, GisCarModelReasonCar.Update,
                    cs.CarInstance.Id, cs.CarInstance.Name,
                    cs.X, cs.Y, cs.ExceptionState);
        }
        private void OnCarUpdate(CarSession cs)
        {
            CarState carState;
            if (!dictCarState.TryGetValue(cs.CarInstance.Id, out carState))
                throw new ApplicationException("CarModel!UpdateError");
            OnCarAllStateUpdate(cs, carState.UserMakeShowable);
        }
        private void OnCarAllStateUpdate(CarSession cs, bool bUserShow)
        {
            CarState carState;
            if (!dictCarState.TryGetValue(cs.CarInstance.Id, out carState))
                throw new ApplicationException("CarModel!UpdateError");
            bool bOldShow = carState.ShowAble;
            carState.Connected = cs.Alive;
            carState.UserMakeShowable = bUserShow;
            if((carState.ShowAble != bOldShow) && (OnCarShowableChanged != null))
                OnCarShowableChanged(this, carState.ShowAble ? GisCarModelReasonCar.Add : GisCarModelReasonCar.Remove,
                    cs.CarInstance.Id, cs.CarInstance.Name,
                    cs.X, cs.Y, cs.ExceptionState);
        }
        
    }
}
