using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using TGis.Common;
using TGis.Viewer.TGisRemote;
using System.ServiceModel;

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
            DevExpress.Utils.AppearanceObject.DefaultFont = new System.Drawing.Font("微软雅黑", 12); 

            try
            {
                NaviHelper.FormStartup = new Login();
                Application.Run(NaviHelper.FormStartup);
            }
            catch (System.ServiceModel.CommunicationException)
            {
                MessageBox.Show("与服务器的连接中断，请检查服务端程序或重试");
                return;
            }
            //catch (System.Exception ex)
            //{
            //    MessageBox.Show("无处理的内部错误");
            //    return;
            //}
            

            GisGlobal.UnInit();
        }
        public static void MainStart()
        {
            Directory.CreateDirectory(Ultility.GetDataDir());
            GisGlobal.Init();
            MainToolModel model = new MainToolModel();
            MainToolController controller = new MainToolController(model);
            NaviHelper.FormMain = new ViewMain2(controller, model);
            NaviHelper.FormMain.Show();
        }
    }
}
