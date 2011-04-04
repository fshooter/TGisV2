using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TGis.RemoteService;

namespace TGis.RemoteHost
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            ServiceController controller = new ServiceController();
            controller.Run();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            controller.Stop();
        }
    }
}
