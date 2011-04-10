using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace TGis.WindowsServiceHost
{
    public partial class GisWinService : ServiceBase
    {
        public GisWinService()
        {
            InitializeComponent();
        }
        TGis.RemoteService.ServiceController controller = null;
        System.ServiceModel.ServiceHost host = null;
        protected override void OnStart(string[] args)
        {
            controller = new TGis.RemoteService.ServiceController();
            controller.Run();
            host = new System.ServiceModel.ServiceHost(typeof(TGis.RemoteService.ServiceImpl));
            host.Open();
        }

        protected override void OnPause()
        {
            if (host != null)
                host.Close();
            host = null;
            base.OnPause();
        }
        protected override void OnContinue()
        {
            host = new System.ServiceModel.ServiceHost(typeof(TGis.RemoteService.ServiceImpl));
            host.Open();
            base.OnContinue();
        }

        protected override void OnStop()
        {
            if (host != null)
                host.Close();
            host = null;
            if(controller != null)
                controller.Stop();
            controller = null;         
            base.OnStop();
        }

        protected override void OnShutdown()
        {
            OnStop();
            base.OnShutdown();
        }
    }
}
