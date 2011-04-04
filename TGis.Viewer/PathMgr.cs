using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using TGis.Viewer.TGisRemote;

namespace TGis.Viewer
{
    class PathMgr
    {
        private IDictionary<int, GisPathInfo> dictPaths = new Dictionary<int, GisPathInfo>();

        public PathMgr()
        {
            ReloadFromSever();
        }
        public GisPathInfo[] Paths
        {
            get 
            {
                GisPathInfo[] r = new GisPathInfo[dictPaths.Count];
                int i = 0;
                lock (this)
                {
                    foreach (GisPathInfo c in dictPaths.Values)
                        r[i++] = c;
                }
                return r;
            }
        }

        public EventHandler OnPathStateChanged;
       
        public bool TryGetPath(int id, out GisPathInfo p)
        {
            lock (this)
            {
                return dictPaths.TryGetValue(id, out p);
            }
        }

        public void RemovePath(int id)
        {
           
        }

        public void UpdatePath(GisPathInfo c)
        {
           
        }

        public void InsertPath(GisPathInfo c)
        {
           
        }

        public void ReloadFromSever()
        {
            var paths = GisServiceWrapper.Instance.GetPathInfo();
            lock (this)
            {
                dictPaths.Clear();
                foreach (var p in paths)
                {
                    dictPaths[p.Id] = p;
                }
            }
            
        }
        private void DispatchStateChangeMsg()
        {
            if (OnPathStateChanged == null)
                return;
            OnPathStateChanged(this, null);
        }
    }
}
