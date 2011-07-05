using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using TGis.Common;
using System.ServiceModel;
using TGis.Viewer.TGisRemote;
using System.Configuration;
using System.ServiceModel.Description;

namespace TGis.Viewer
{
    class GisServiceWrapper : IGisServiceAblity
    {
        static GisServiceWrapper instance = null;
        ChannelFactory<IGisServiceAblity> channelFactory;
        private GisServiceWrapper()
        {
            ContractDescription contract = ContractDescription.GetContract(typeof(TGisRemote.IGisServiceAblity));
            EndpointAddress address = new EndpointAddress(GisGlobal.GetServerUri());
            var binding = new NetTcpBinding();
            binding.Security.Mode = SecurityMode.None;
            ServiceEndpoint endpoint = new ServiceEndpoint(contract, binding, address);
            channelFactory = new ChannelFactory<IGisServiceAblity>(endpoint);
            channelFactory.Open();
        }
        public static GisServiceWrapper Instance
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
        public GisSessionInfo[] QuerySessionInfo(out DateTime tmCursor, DateTime tmStart, DateTime tmEnd)
        {
            var proxy = channelFactory.CreateChannel();
            using (proxy as IDisposable)
            {
                return proxy.QuerySessionInfo(out tmCursor,tmStart, tmEnd);
            }
        }
        public bool AddCarInfo(GisCarInfo info)
        {
            //var proxy = channelFactory.CreateChannel();
            //using (proxy as IDisposable)
            //{
            //    return proxy.AddCarInfo(info);
            //}
            return false;
        }

        public bool UpdateCarInfo(GisCarInfo info)
        {
            //var proxy = channelFactory.CreateChannel();
            //using (proxy as IDisposable)
            //{
            //    return proxy.UpdateCarInfo(info);
            //}
            return false;
        }

        public bool RemoveCarInfo(int id)
        {
            //var proxy = channelFactory.CreateChannel();
            //using (proxy as IDisposable)
            //{
            //    return proxy.RemoveCarInfo(id);
            //}
            return false;
        }

        public bool AddPathInfo(GisPathInfo info)
        {
            //var proxy = channelFactory.CreateChannel();
            //using (proxy as IDisposable)
            //{
            //    return proxy.AddPathInfo(info);
            //}
            return false;
        }

        public bool UpdatePathInfo(GisPathInfo info)
        {
            //var proxy = channelFactory.CreateChannel();
            //using (proxy as IDisposable)
            //{
            //    return proxy.UpdatePathInfo(info);
            //}
            return false;
        }

        public bool RemovePathInfo(int id)
        {
            //var proxy = channelFactory.CreateChannel();
            //using (proxy as IDisposable)
            //{
            //    return proxy.RemovePathInfo(id);
            //}
            return false;
        }

        public GisEventInfo[] QueryEventInfo( out bool bTobeContinue, DateTime tmStart, DateTime tmEnd, int startId)
        {
            var proxy = channelFactory.CreateChannel();
            using (proxy as IDisposable)
            {
                return proxy.QueryEventInfo(out bTobeContinue, tmStart, tmEnd, startId);
            }
        }

        public bool VerifyPassword(byte[] pass)
        {
            var proxy = channelFactory.CreateChannel();
            using (proxy as IDisposable)
            {
                return proxy.VerifyPassword(pass);
            }
        }
        public void ModifyPassword(byte[] pass)
        {
            //var proxy = channelFactory.CreateChannel();
            //using (proxy as IDisposable)
            //{
            //    proxy.ModifyPassword(pass);
            //}
            return;
        }
        public bool QueryCarDetail(out GisCarDetail detail, int id)
        {
            var proxy = channelFactory.CreateChannel();
            using (proxy as IDisposable)
            {
                return proxy.QueryCarDetail(out detail, id);
            }
        }
        public bool UpdateCarDetail(GisCarDetail detail)
        {
            //var proxy = channelFactory.CreateChannel();
            //using (proxy as IDisposable)
            //{
            //    return proxy.UpdateCarDetail(detail);
            //}
            return false;
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
        public static bool GetCurrentIsAdmin()
        {
            bool br = false;
            try
            {
                System.Configuration.Configuration config =
                    ConfigurationManager.OpenExeConfiguration(
                    ConfigurationUserLevel.None);
                AppSettingsSection appSection = (AppSettingsSection)config.GetSection("appSettings");
                if (appSection.Settings["Admin"].Value == "true")
                    br = true;
            }
            catch (System.Exception ex)
            {
                br = false;
            }
            return br;
        }
        public static string GetServerUri()
        {
            string addr;
            int port;
            System.Configuration.Configuration config =
                    ConfigurationManager.OpenExeConfiguration(
                    ConfigurationUserLevel.None);
            AppSettingsSection appSection = (AppSettingsSection)config.GetSection("appSettings");
            addr = appSection.Settings["ServerAddr"].Value;
            port = Convert.ToInt32(appSection.Settings["ServerPort"].Value);
            string url = string.Format("net.tcp://{0}:{1}/TGisService/100",
                addr,
                port);
            return url;
        }
        public static bool GetServerConnectionConf(out string addr, out int port)
        {
            bool br = true;
            addr = "";
            port = 0;
            try
            {
                System.Configuration.Configuration config =
                    ConfigurationManager.OpenExeConfiguration(
                    ConfigurationUserLevel.None);
                AppSettingsSection appSection = (AppSettingsSection)config.GetSection("appSettings");
                addr = appSection.Settings["ServerAddr"].Value;
                port = Convert.ToInt32(appSection.Settings["ServerPort"].Value);
            }
            catch (System.Exception ex)
            {
                br = false;
            }
            return br;
        }
        public static void SetServerConnectionConf(string addr, int port)
        {
            System.Configuration.Configuration config =
                ConfigurationManager.OpenExeConfiguration(
                ConfigurationUserLevel.None);
            AppSettingsSection appSection = (AppSettingsSection)config.GetSection("appSettings");
            appSection.Settings["ServerAddr"].Value = addr;
            appSection.Settings["ServerPort"].Value = port.ToString();
            config.Save();
        }
        public static string GetGoogleMapPath()
        {
            return Ultility.GetAppDir() + "\\map\\" + GetAllMaps()["谷歌地图"];
        }
        public static string GetSelectedMapPath()
        {
            return Ultility.GetAppDir() + "\\map\\" + GetAllMaps()[GetSelectedMapName()];
        }
        public static string GetSelectedMapName()
        {
            string mapName;
            try
            {
                System.Configuration.Configuration config =
                    ConfigurationManager.OpenExeConfiguration(
                    ConfigurationUserLevel.None);
                AppSettingsSection appSection = (AppSettingsSection)config.GetSection("appSettings");
                mapName = appSection.Settings["MapName"].Value;
            }
            catch (System.Exception ex)
            {
                mapName = "谷歌地图";
            }
            return mapName;
        }
        public static void SetSelectedMapName(string name)
        {
            try
            {
                System.Configuration.Configuration config =
                    ConfigurationManager.OpenExeConfiguration(
                    ConfigurationUserLevel.None);
                AppSettingsSection appSection = (AppSettingsSection)config.GetSection("appSettings");
                appSection.Settings["MapName"].Value = name;
                config.Save();
            }
            catch (System.Exception)
            {
                
            }
        }
        public static IDictionary<string, string> GetAllMaps()
        {
            IDictionary<string, string> r = new Dictionary<string, string>();
            r["谷歌地图"] = "mapg.html";
            r["百度地图"] = "mapb.html";
            return r;
        }
    }
}
