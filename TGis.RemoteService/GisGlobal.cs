﻿using System;
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
        public static CarSessionMgr GImmCarSessionMgr;
        public static IDbConnection GConnection;
        public static CarSessionLogger GSessionLogger;
        public static HistoryCarSession GCarSessionQueryer;

        public static void Init()
        {
            OpenDb();
            GCarMgr = new CarMgr(GConnection);
            GPathMgr = new PathMgr(GConnection);
            GImmCarSessionMgr = new CarSessionMgr(GCarMgr, GPathMgr);
            GCarSessionQueryer = new HistoryCarSession(GConnection);
            ICarTerminalAbility immTerminal = new TestCarTerminalAbility();
            GImmCarSessionMgr.Terminal = immTerminal;

            GSessionLogger = new CarSessionLogger(GImmCarSessionMgr, GConnection);
            GSessionLogger.Run(2000);
            immTerminal.Run();
        }
        public static void UnInit()
        {
            GImmCarSessionMgr.Terminal.Stop();
            GImmCarSessionMgr.Stop();
            GSessionLogger.Stop();
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