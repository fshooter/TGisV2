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
                infos[i].Id = c.Id;
                infos[i].Name = c.Name;
                infos[i].PathId = c.PathId;
                i++;
            }
            return infos;
        }

        public GisPathInfo[] GetPathInfo()
        {
            GisPathInfo path = new GisPathInfo();
            path.Id = 1;
            path.Name = "testPath";
            path.Points = new double[] { 1, 1, 2, 2, 3, 3 };
            return new GisPathInfo[] { path };
        }

        public GisSessionInfo[] QuerySessionInfo(DateTime tmStart, DateTime tmEnd)
        {
            return GisGlobal.GCarSessionQueryer.Query(tmStart, tmEnd);
        }
    }
}
