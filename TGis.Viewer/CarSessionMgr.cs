using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using TGis.Common;
using System.Diagnostics;
using TGis.Viewer.TGisRemote;
using System.ComponentModel;
using System.Windows.Forms;

namespace TGis.Viewer
{
    delegate void CarSessionMsgHandler(object sender, GisSessionInfo sessionInfo);
    class CarSessionMgr
    {
        int interval;
        int multiply;
        System.Timers.Timer timer = new System.Timers.Timer();
        DateTime currentTime;
        bool bImmMode;
        TimeSpan delayTime = new TimeSpan(0, 0, 10);
        int totalFailTime = 0;
        
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
                currentTime = DateTime.MaxValue;
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
            if (totalFailTime > 10) return;
            try
            {
                if (OnBeginQuerySessionMsg != null)
                    OnBeginQuerySessionMsg(this, null);
                DateTime endTime;

                if (bImmMode)
                    endTime = DateTime.MaxValue;
                else
                    endTime = currentTime.AddMilliseconds(interval * multiply);
                var sessionMsgs = GisServiceWrapper.Instance.QuerySessionInfo(out currentTime, currentTime, endTime);
                foreach (var msg in sessionMsgs)
                {
                    SessionMsgHandler(msg);
                }
                if (!bImmMode)
                    currentTime = endTime;
                totalFailTime = 0;
            }
            catch (System.Exception ex)
            {
                totalFailTime++;
                if (totalFailTime > 10)
                {
                    MessageBox.Show("与服务器的连接中断，请检查服务器或重试");
                }
            }
        }

        private void SessionMsgHandler(GisSessionInfo msg)
        {
            if (OnSessionMsgReceived != null)
                OnSessionMsgReceived(this, msg);
        }
    }
}
