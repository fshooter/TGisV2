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
        public void ImmediateMode()
        {
            GisCarModel model = new GisCarModel();
            model.CsMgr = GisGlobal.GImmCarSessionMgr;
            GisCarController controller = new GisCarController();
            Form viewGisCar = new ViewGisCar(model, controller);
            NaviHelper.NaviTo(viewGisCar);
        }
    }
}
