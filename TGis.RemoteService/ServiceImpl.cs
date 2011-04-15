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

        public bool VerifyPassword(byte[] pass)
        {
            return GisGlobal.GPassMgr.VerifyPass(pass);
        }

        public void ModifyPassword(byte[] newPass)
        {
            GisGlobal.GPassMgr.ModifyPass(newPass);
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
                info.SerialNum = c.SerialNum;
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

        public GisSessionInfo[] QuerySessionInfo(DateTime tmStart, DateTime tmEnd, out DateTime tmCorser)
        {
            return GisGlobal.GCarSessionQueryer.Query(tmStart, tmEnd, out tmCorser);
        }

        public bool AddCarInfo(GisCarInfo info)
        {
            bool br = true;
            try
            {
                GisGlobal.GCarMgr.InsertCar(new Car(info.Id, info.Name, info.SerialNum, info.PathId));
            }
            catch (System.Exception)
            {
                br = false;
            }
            return br;
        }

        public bool UpdateCarInfo(GisCarInfo info)
        {
            bool br = true;
            try
            {
                GisGlobal.GCarMgr.UpdateCar(new Car(info.Id, info.Name, info.SerialNum, info.PathId));
            }
            catch (System.Exception)
            {
                br = false;
            }
            return br;
        }

        public bool RemoveCarInfo(int id)
        {
            bool br = true;
            try
            {
                GisGlobal.GCarMgr.RemoveCar(new Car(id, "", "", -1));
            }
            catch (System.Exception)
            {
                br = false;
            }
            return br;
        }

        public bool AddPathInfo(GisPathInfo info)
        {
            bool br = true;
            try
            {
                GisGlobal.GPathMgr.InsertPath(new Path(info.Id, info.Name, info.Points));
            }
            catch (System.Exception)
            {
                br = false;
            }
            return br;
        }

        public bool UpdatePathInfo(GisPathInfo info)
        {
            bool br = true;
            try
            {
                GisGlobal.GPathMgr.UpdatePath(new Path(info.Id, info.Name, info.Points));
            }
            catch (System.Exception)
            {
                br = false;
            }
            return br;
        }

        public bool RemovePathInfo(int id)
        {
            bool br = true;
            try
            {
                Path p = new Path();
                p.Id = id;
                GisGlobal.GPathMgr.RemovePath(p);
            }
            catch (System.Exception)
            {
                br = false;
            }
            return br;
        }

        public GisEventInfo[] QueryEventInfo(out bool bTobeContinue, DateTime tmStart, DateTime tmEnd, int startId)
        {
            return GisGlobal.GEventQueryer.Query(out bTobeContinue, tmStart, tmEnd, startId);
        }
    }
}
