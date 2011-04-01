using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using TGis.Common;

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
            Directory.CreateDirectory(Ultility.GetDataDir());
            GisGlobal.Init();
            MainToolModel model = new MainToolModel();
            MainToolController controller = new MainToolController(model);
            NaviHelper.FormMain = new ViewMain(controller, model);
            Application.Run(NaviHelper.FormMain);

            GisGlobal.UnInit();
        }
        
    }
}
