using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TGis.RemoteContract;

namespace TGis.RemoteService
{
    public class ServiceImpl : IGisServiceAblity
    {
        public int GetVersion()
        {
            return 100; // 1.0.0
        }

        public DateTime GetCurrentTime()
        {
            return DateTime.Now;
        }

        public GisCarInfo[] GetCarInfo()
        {
            Car[] cars = GisGlobal.GCarMgr.Cars;
            GisCarInfo[] infos = new GisCarInfo[cars.Length];
            int i = 0;
            foreach (Car c in cars)
            {
                GisCarInfo info = new GisCarInfo();
                info.Id = c.Id;
                info.Name = c.Name;
                info.PathId = c.PathId;
                infos[i++] = info;
            }
            return infos;
        }

        public GisPathInfo[] GetPathInfo()
        {
            Path[] paths = GisGlobal.GPathMgr.Paths;
            GisPathInfo[] infos = new GisPathInfo[paths.Length];
            int i = 0;
            foreach (var c in paths)
            {
                GisPathInfo info = new GisPathInfo();
                info.Id = c.Id;
                info.Name = c.Name;
                info.Points = c.PathPolygon.Points;
                infos[i++] = info;
            }
            return infos;
        }

        public GisSessionInfo[] QuerySessionInfo(DateTime tmStart, DateTime tmEnd)
        {
            return GisGlobal.GCarSessionQueryer.Query(tmStart, tmEnd);
        }
    }
}
