using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TGis.Viewer
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainToolModel model = new MainToolModel();
            MainToolController controller = new MainToolController(model);
            NaviHelper.FormMain = new ViewMain(controller, model);
            Application.Run(NaviHelper.FormMain);
        }
    }
}
