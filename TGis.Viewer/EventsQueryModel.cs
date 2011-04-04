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
        public void Query(DateTime tmStart, DateTime tmEnd)
        {
            bool bTobeContinue = false;
            Events = GisServiceWrapper.Instance.QueryEventInfo(out bTobeContinue, tmStart, tmEnd);
        }

        public event EventHandler OnEventsChanged;
        public event EventHandler OnEventSelected;
    }
}
