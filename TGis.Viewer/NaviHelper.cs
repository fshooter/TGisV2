using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TGis.Viewer
{
    class NaviHelper
    {
        private static Form formMain = null;

        public static Form FormMain
        {
            get { return NaviHelper.formMain; }
            set { NaviHelper.formMain = value; }
        }
        public static void NaviTo(Form formToShow)
        {
            formToShow.MdiParent = formMain;
            formToShow.TopMost = true;
            formToShow.WindowState = FormWindowState.Maximized;
            formToShow.Show();
        }
    }
}
