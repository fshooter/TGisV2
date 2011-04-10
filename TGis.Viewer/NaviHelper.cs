using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TGis.Viewer
{
    class NaviHelper
    {
        private static Form formStartup = null;
        private static ViewMain2 formMain = null;
        private static Form formCurrent = null;

        public static Form FormStartup
        {
            get { return NaviHelper.formStartup; }
            set { NaviHelper.formStartup = value; }
        }

        public static ViewMain2 FormMain
        {
            get { return NaviHelper.formMain; }
            set { NaviHelper.formMain = value; }
        }
        public static void NaviTo(Form formToShow)
        {
            if (formCurrent != null)
            {
                formCurrent.Close();
                formCurrent.Dispose();
            }
            formToShow.MdiParent = formMain;
            formToShow.TopMost = true;
            formToShow.WindowState = FormWindowState.Maximized;
            formToShow.Show();
            formCurrent = formToShow;
        }
        public static void NaviToWelcome()
        {
            Form fm = new ViewWelcome2();
            NaviTo(fm);
        }
        public static void NaviToModifyPath(int pid)
        {
            ModifyPathModel model = new ModifyPathModel(pid);
            Form fm = new ViewModifyPath2(model);
            NaviTo(fm);
        }
        public static void NaviToModifyCar(int cid)
        {
            Form fm = new ViewModifyCar2(cid);
            NaviTo(fm);
        }
    }
}
