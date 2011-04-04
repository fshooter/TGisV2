using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TGis.RemoteService
{
    public class ServiceController
    {

        public void Run()
        {
            GisGlobal.Init();
            
        }
        public void Stop()
        {
            GisGlobal.UnInit();
        }
    }
}
