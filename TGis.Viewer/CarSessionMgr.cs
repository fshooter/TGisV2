using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using TGis.Common;
using System.Diagnostics;
using TGis.Viewer.TGisRemote;
using System.ComponentModel;

namespace TGis.Viewer
{
    delegate void CarSessionMsgHandler(object sender, GisSessionInfo sessionInfo);
    class CarSessionMgr
    {
        int interval;
        int multiply;
        Timer timer = new Timer();
        DateTime currentTime;
        bool bImmMode;
        TimeSpan delayTime = new TimeSpan(0, 0, 10);
        
        public bool ImmMode
        {
            get { return bImmMode; }
        }
        public int Multiply
        {
            get { return multiply; }
            set { multiply = value; }
        }
        public ISynchronizeInvoke SynObject
        {
            get { return timer.SynchronizingObject; }
            set { timer.SynchronizingObject = value; }
        }

        public DateTime CurrentTime
        {
          get { return currentTime; }
          set {
              if (bImmMode)
                  throw new ApplicationException("Can't set time in ImmMode");
              currentTime = value;
          }
        }
        public CarSessionMsgHandler OnSessionMsgReceived;
        public EventHandler OnBeginQuerySessionMsg;
        public CarSessionMgr(bool bImmMode)
        {
            this.bImmMode = bImmMode;
            if (this.bImmMode)
            {
                currentTime = GisServiceWrapper.Instance.GetCurrentTime() - delayTime;
            }
            timer.Elapsed += new ElapsedEventHandler(QueryCarStates);
        }
        public void Run(int interval, int multiply = 1)
        {
            if (bImmMode && (multiply != 1))
                throw new ApplicationException("Multiply must be 1 in ImmMode");
            this.interval = interval;
            this.multiply = multiply;
            timer.Interval = interval;
            timer.Start();
        }
        public void Stop()
        {
            timer.Stop();
            timer.Close();
        }
        private void QueryCarStates(object sender, EventArgs e)
        {
            if (OnBeginQuerySessionMsg != null)
                OnBeginQuerySessionMsg(this, null);
            DateTime endTime;

            if (bImmMode)
                endTime = GisServiceWrapper.Instance.GetCurrentTime() - delayTime;
            else
                endTime = currentTime.AddMilliseconds(interval * multiply);
            var sessionMsgs = GisServiceWrapper.Instance.QuerySessionInfo(currentTime, endTime);
            foreach (var msg in sessionMsgs)
            {
                SessionMsgHandler(msg);
            }
            currentTime = endTime;
        }

        private void SessionMsgHandler(GisSessionInfo msg)
        {
            if (OnSessionMsgReceived != null)
                OnSessionMsgReceived(this, msg);
        }
    }
}
