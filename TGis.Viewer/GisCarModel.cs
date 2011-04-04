using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TGis.Viewer
{
    class GisCarModel
    {
        CarSessionMgr csm;
        IDictionary<int, bool> dictCarShow = new Dictionary<int, bool>();
        IDictionary<int, bool> dictPathShow = new Dictionary<int, bool>();

        internal CarSessionMgr SessionMgr
        {
            get { return csm; }
        }
        public GisCarModel(CarSessionMgr csm)
        {
            this.csm = csm;
        }
        public void Stop()
        {
            csm.Stop();
        }
        public EventHandler OnPathShowChanged;
        public EventHandler OnCarShowChanged;
        public void UserMakeCarShow(int id, bool bShow)
        {
            dictCarShow[id] = bShow;
            if (OnCarShowChanged != null)
                OnCarShowChanged(this, null);
        }
        public bool GetCarShow(int id)
        {
            bool bShow;
            if (dictCarShow.TryGetValue(id, out bShow))
                return bShow;
            return false;
        }

        public void UserMakePathShow(int id, bool bShow)
        {
            dictPathShow[id] = bShow;
            if (OnPathShowChanged != null)
                OnPathShowChanged(this, null);
        }
        public bool GetPathShow(int id)
        {
            bool bShow;
            if (dictPathShow.TryGetValue(id, out bShow))
                return bShow;
            return false;
        }
    }
}
