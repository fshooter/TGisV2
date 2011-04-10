using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SQLite;
using TGis.Common;

namespace TGis.RemoteService
{
    class GisGlobal
    {
        public static CarMgr GCarMgr;
        public static PathMgr GPathMgr;
        public static PassMgr GPassMgr;
        public static CarSessionMgr GImmCarSessionMgr;
        public static IDbConnection GConnection;
        public static CarSessionLogger GSessionLogger;
        public static HistoryCarSession GCarSessionQueryer;
        public static CarEventLogger GEventLogger;
        public static CarEventQueryer GEventQueryer;
        private static TaskSchduler GTasks;

        public static void Init()
        {
            OpenDb();
            GCarMgr = new CarMgr(GConnection);
            GPathMgr = new PathMgr(GConnection);
            GPassMgr = new PassMgr(GConnection);
            GImmCarSessionMgr = new CarSessionMgr(GCarMgr, GPathMgr);
            GCarSessionQueryer = new HistoryCarSession(GConnection);
            //ICarTerminalAbility immTerminal = new TestCarTerminalAbility();
            ICarTerminalAbility immTerminal = new UdpCarTerminalAbility();
            GImmCarSessionMgr.Terminal = immTerminal;

            GSessionLogger = new CarSessionLogger(GImmCarSessionMgr, GConnection);
            GSessionLogger.Run(2000);

            GEventLogger = new CarEventLogger(GisGlobal.GConnection,
                GisGlobal.GImmCarSessionMgr);
            GEventLogger.Run();

            immTerminal.Run();
            GEventQueryer = new CarEventQueryer(GConnection);

            GTasks = new TaskSchduler();
            GTasks.Run(GConnection, Ultility.GetDataDir() + "\\GisDb.db");
        }
        public static void UnInit()
        {
            GTasks.Stop();
            GEventLogger.Stop();
            GImmCarSessionMgr.Terminal.Stop();
            GImmCarSessionMgr.Stop();
            GSessionLogger.Stop();
        }
        private static void OpenDb()
        {
            IDbConnection conn = new System.Data.SQLite.SQLiteConnection(
                String.Format(@"Data Source={0}\GisDb.db;Pooling=true;FailIfMissing=false;Synchronous=Off;Compress=True",
                Ultility.GetDataDir()));
            conn.Open();
            GConnection = conn;
        }
    }
}
