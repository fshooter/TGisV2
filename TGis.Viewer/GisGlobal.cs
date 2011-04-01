using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SQLite;
using TGis.Common;

namespace TGis.Viewer
{
    class GisGlobal
    {
        public static CarMgr GCarMgr;
        public static PathMgr GPathMgr;
        public static CarSessionMgr GImmCarSessionMgr;
        public static IDbConnection GConnection;

        public static void Init()
        {
            OpenDb();
            GCarMgr = new CarMgr(GConnection);
            GPathMgr = new PathMgr(GConnection);
            GImmCarSessionMgr = new CarSessionMgr(GCarMgr);
            ICarTerminalAbility immTerminal = new TestCarTerminalAbility();
            GImmCarSessionMgr.Terminal = immTerminal;
            immTerminal.Run();
        }
        public static void UnInit()
        {
            GImmCarSessionMgr.Terminal.Stop();
            GImmCarSessionMgr.Stop();
        }
        private static void OpenDb()
        {
            IDbConnection conn = new System.Data.SQLite.SQLiteConnection(
                String.Format(@"Data Source={0}\GisDb.db;Pooling=true;FailIfMissing=false",
                Ultility.GetDataDir()));
            conn.Open();
            GConnection = conn;
        }
    }
}
