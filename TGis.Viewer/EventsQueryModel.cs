using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TGis.Viewer.TGisRemote;

namespace TGis.Viewer
{
    class EventsQueryModel
    {
        GisEventInfo[] events;
        private GisEventInfo eventSelected;
        bool bTobeContinue = false;
        int curPageNum = 0;
        int eventsPerPage = 0;
        DateTime tmStart, tmEnd;

        public GisEventInfo EventSelected
        {
          get { return eventSelected; }
          set { 
              eventSelected = value; 
              if(OnEventSelected != null)
                  OnEventSelected(this, null);
          }
        }

        public GisEventInfo[] Events
        {
            get { return events; }
            set {
                events = value; 
                if(OnEventsChanged != null)
                  OnEventsChanged(this, null);
            }
        }
        public void Query(DateTime tmStart, DateTime tmEnd, int startId = 0)
        {
            this.tmStart = tmStart;
            this.tmEnd = tmEnd;
            bTobeContinue = false;
            if (startId == 0) curPageNum = 0;
            Events = GisServiceWrapper.Instance.QueryEventInfo(out bTobeContinue, tmStart, tmEnd, startId);
            eventsPerPage = Events.Length;
        }
        public void QueryNext()
        {
            curPageNum++;
            Query(tmStart, tmEnd, eventsPerPage * curPageNum);
        }

        public void QueryPre()
        {
            if (curPageNum <= 0)
                return;
            curPageNum--;
            Query(tmStart, tmEnd, eventsPerPage * curPageNum);
        }
        public bool CanQueryNext
        {
            get { return bTobeContinue; }
        }
        public bool CanQueryPre
        {
            get { return curPageNum > 0; }
        }

        public event EventHandler OnEventsChanged;
        public event EventHandler OnEventSelected;
    }
}
