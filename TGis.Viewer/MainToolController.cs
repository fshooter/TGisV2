using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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
            Form viewGisCar = new ViewGisCar();
            NaviHelper.NaviTo(viewGisCar);
        }
    }
}
