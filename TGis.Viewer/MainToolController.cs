using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TGis.Common;

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
            GisCarModel model = new GisCarModel(GisGlobal.GImmCarSessionMgr);
            GisCarController controller = new GisCarController();
            Form viewGisCar = new ViewGisCar2(model, controller);
            NaviHelper.NaviTo(viewGisCar);
        }
        public void CreateNewPath()
        {
            string newName = null;
            for (int i = 1; i < 20; i++)
            {
                string testName = string.Format("新建路径-{0}", i);
                bool bValid = true;
                foreach(Path p in GisGlobal.GPathMgr.Paths)
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
            Path newp = new Path();
            newp.Name = newName;
            GisGlobal.GPathMgr.InsertPath(newp);
        }
        public void CreateNewCar()
        {
            string newName = null;
            for (int i = 1; i < 20; i++)
            {
                string testName = string.Format("新建车辆-{0}", i);
                bool bValid = true;
                foreach (Car c in GisGlobal.GCarMgr.Cars)
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
            Car newp = new Car();
            newp.Name = newName;
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
    }
}
