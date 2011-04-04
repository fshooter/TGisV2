using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SQLite;
using TGis.Common;
using System.ServiceModel;
using TGis.Viewer.TGisRemote;

namespace TGis.Viewer
{
    class GisServiceWrapper : IGisServiceAblity
    {
        static GisServiceWrapper instance = null;
        ChannelFactory<IGisServiceAblity> channelFactory;
        private GisServiceWrapper()
        {
            channelFactory = new ChannelFactory<IGisServiceAblity>("NetTcpBinding_IGisServiceAblity");
        }
        public static IGisServiceAblity Instance
        {
            get
            {
                if (instance == null)
                    instance = new GisServiceWrapper();
                return instance;
            }
            set
            {
                if (instance != null)
                    instance.Close();
                instance = null;
            }
        }
        public void Close()
        {
            channelFactory.Close();
        }
        public int GetVersion()
        {
            var proxy = channelFactory.CreateChannel();
            using (proxy as IDisposable)
            {
                return proxy.GetVersion();
            }
        }
        public DateTime GetCurrentTime()
        {
            var proxy = channelFactory.CreateChannel();
            using (proxy as IDisposable)
            {
                return proxy.GetCurrentTime();
            }
        }
        public GisCarInfo[] GetCarInfo()
        {
            var proxy = channelFactory.CreateChannel();
            using (proxy as IDisposable)
            {
                return proxy.GetCarInfo();
            }
        }
        public GisPathInfo[] GetPathInfo()
        {
            var proxy = channelFactory.CreateChannel();
            using (proxy as IDisposable)
            {
                return proxy.GetPathInfo();
            }
        }
        public GisSessionInfo[] QuerySessionInfo(DateTime tmStart, DateTime tmEnd)
        {
            var proxy = channelFactory.CreateChannel();
            using (proxy as IDisposable)
            {
                return proxy.QuerySessionInfo(tmStart, tmEnd);
            }
        }
        public bool AddCarInfo(GisCarInfo info)
        {
            var proxy = channelFactory.CreateChannel();
            using (proxy as IDisposable)
            {
                return proxy.AddCarInfo(info);
            }
        }

        public bool UpdateCarInfo(GisCarInfo info)
        {
            var proxy = channelFactory.CreateChannel();
            using (proxy as IDisposable)
            {
                return proxy.UpdateCarInfo(info);
            }
        }

        public bool RemoveCarInfo(int id)
        {
            var proxy = channelFactory.CreateChannel();
            using (proxy as IDisposable)
            {
                return proxy.RemoveCarInfo(id);
            }
        }

        public bool AddPathInfo(GisPathInfo info)
        {
            var proxy = channelFactory.CreateChannel();
            using (proxy as IDisposable)
            {
                return proxy.AddPathInfo(info);
            }
        }

        public bool UpdatePathInfo(GisPathInfo info)
        {
            var proxy = channelFactory.CreateChannel();
            using (proxy as IDisposable)
            {
                return proxy.UpdatePathInfo(info);
            }
        }

        public bool RemovePathInfo(int id)
        {
            var proxy = channelFactory.CreateChannel();
            using (proxy as IDisposable)
            {
                return proxy.RemovePathInfo(id);
            }
        }
    }
    class GisGlobal
    {
        public static CarMgr GCarMgr;
        public static PathMgr GPathMgr;       

        public static void Init()
        {
            GCarMgr = new CarMgr();
            GPathMgr = new PathMgr();
        }
        public static void UnInit()
        {
            
        }
    }
}
