using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TGis.Common;
using TGis.Viewer.TGisRemote;
using System.Security.Cryptography;

namespace TGis.Viewer
{
    class MainToolController
    {
        private MainToolModel model;
        public MainToolController(MainToolModel model)
        {
            this.model = model;
        }
        public void WelcomeMode()
        {
            NaviHelper.NaviToWelcome();
        }
        public void ImmediateMode()
        {
            CarSessionMgr csm = new CarSessionMgr(true);
            GisCarModel model = new GisCarModel(csm);
            GisCarController controller = new GisCarController();
            Form viewGisCar = new ViewGisCar2(model, controller);
            csm.SynObject = viewGisCar;
            csm.Run(2000);
            NaviHelper.NaviTo(viewGisCar);
        }
        public void HistoryMode()
        {
            CarSessionMgr csm = new CarSessionMgr(false);
            csm.CurrentTime = DateTime.Now - new TimeSpan(1, 0, 0, 0);
            GisCarModel model = new GisCarModel(csm);
            GisCarController controller = new GisCarController();
            Form viewGisCar = new ViewGisCar2(model, controller);
            csm.SynObject = viewGisCar;
            csm.Run(2000);
            NaviHelper.NaviTo(viewGisCar);
        }
        public void EventsMode()
        {
            EventsQueryModel model = new EventsQueryModel();
            Form viewEvents = new ViewEvents(model);
            NaviHelper.NaviTo(viewEvents);
        }
        public void CreateNewPath()
        {
            GisGlobal.GPathMgr.ReloadFromSever();
            string newName = null;
            for (int i = 1; i < 20; i++)
            {
                string testName = string.Format("新建路径-{0}", i);
                bool bValid = true;
                foreach(GisPathInfo p in GisGlobal.GPathMgr.Paths)
                {
                    if (p.Name == testName)
                    {
                        bValid = false;
                        break;
                    }
                }
                if (bValid)
                {
                    newName = testName;
                    break;
                }
            }
            if (newName == null) return;
            GisPathInfo newp = new GisPathInfo();
            newp.Name = newName;
            newp.Points = new double[] { 0, 0, 1, 1, 2, 2 };
            GisGlobal.GPathMgr.InsertPath(newp);
        }
        public void CreateNewCar()
        {
            GisGlobal.GCarMgr.ReloadFromSever();
            string newName = null;
            for (int i = 1; i < 30; i++)
            {
                string testName = string.Format("新建车辆-{0}", i);
                bool bValid = true;
                foreach (GisCarInfo c in GisGlobal.GCarMgr.Cars)
                {
                    if (c.Name == testName)
                    {
                        bValid = false;
                        break;
                    }
                }
                if (bValid)
                {
                    newName = testName;
                    break;
                }
            }
            if (newName == null) return;
            GisCarInfo newp = new GisCarInfo();
            newp.Name = newName;
            newp.PathId = -1;
            GisGlobal.GCarMgr.InsertCar(newp);
        }
        public void ModifyPath(int pid)
        {
            NaviHelper.NaviToModifyPath(pid);
        }
        public void ModifyCar(int cid)
        {
            NaviHelper.NaviToModifyCar(cid);
        }

        public void ModifyPass(string pass)
        {
            if ((pass == null) || (pass.Length == 0))
                throw new ApplicationException("密码不能为空");
            SHA1 hash = SHA1.Create();
            byte[] newpass = hash.ComputeHash(Encoding.Default.GetBytes(pass));
            GisServiceWrapper.Instance.ModifyPassword(newpass);
        }
    }
}
