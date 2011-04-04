using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using TGis.Common;
using System.Threading;
using TGis.Viewer.TGisRemote;

namespace TGis.Viewer
{
    class CarMgr
    {
        private IDictionary<int, GisCarInfo> dictCars = new Dictionary<int, GisCarInfo>();

        public CarMgr()
        {
            ReloadFromSever();
        }
        public GisCarInfo[] Cars
        {
            get 
            {
                GisCarInfo[] r = new GisCarInfo[dictCars.Count];
                int i = 0;
                lock (this)
                {
                    foreach (GisCarInfo c in dictCars.Values)
                        r[i++] = c;
                }
                return r;
            }
        }
        
        public EventHandler OnCarStateChanged = null;

        public bool TryGetCar(int id, out GisCarInfo c)
        {
            lock (this)
            {
                return dictCars.TryGetValue(id, out c);
            }
        }
        public void RemoveCar(int id)
        {
           
        }
        public void UpdateCar(GisCarInfo c)
        {
            
        }

        public void InsertCar(GisCarInfo c)
        {
            
        }
        public void ReloadFromSever()
        {
            GisCarInfo[] cars = GisServiceWrapper.Instance.GetCarInfo();
            lock (this)
            {
                dictCars.Clear();
                foreach (var c in cars)
                {
                    dictCars[c.Id] = c;
                }
            }
        }

        private void DispatchStateChangeMsg()
        {
            if (OnCarStateChanged == null)
                return;
            OnCarStateChanged(this, null);
        }
    }
}
