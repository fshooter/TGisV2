using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TGis.Common;
using System.Timers;
using System.Data;
using TGis.RemoteContract;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Data.SQLite;

namespace TGis.RemoteService
{
    class CarSessionLogger
    {
        CarSessionMgr csm;
        IList<GisSessionInfo> listCachedMsg = new List<GisSessionInfo>();
        Timer timer = new Timer();
        IDbConnection conn;
        bool bRunning = false;
        public CarSessionLogger(CarSessionMgr csm, IDbConnection connection)
        {
            this.csm = csm;
            conn = connection;
            using (IDbCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"create table if not exists csmmsg 
                    (time INTEGER, data BLOB)";
                cmd.ExecuteNonQuery();
                cmd.CommandText = @"create index if not exists csmmsg_index 
                    on csmmsg (time)";
                cmd.ExecuteNonQuery();
            }
            timer.Elapsed += new ElapsedEventHandler(FlushCachedMsg);

        }
        public void Run(int interval)
        {
            listCachedMsg.Clear();
            bRunning = true;
            timer.AutoReset = true;
            timer.Interval = interval;
            timer.Start();
            csm.OnCarSessionStateChanged += new CarSessionStateChangeHandler(SessionMsgHandler);
        }
        public void Stop()
        {
            bRunning = false;
            csm.OnCarSessionStateChanged -= new CarSessionStateChangeHandler(SessionMsgHandler);
            timer.Stop();
        }

        private void SessionMsgHandler(object sender, CarSessionStateChangeArgs arg)
        {
            if (!bRunning) return;
            lock (this)
            {
                listCachedMsg.Add(ConvertSessionMsg(arg));
            }
            return;
        }

        private GisSessionInfo ConvertSessionMsg(CarSessionStateChangeArgs arg)
        {
            GisSessionInfo info = new GisSessionInfo();
            info.CarId = arg.CarSessionArg.CarInstance.Id;
            info.Alive = arg.CarSessionArg.Alive;
            info.RoolDirection = (arg.CarSessionArg.RollDirection == CarRollDirection.Forward ? true : false);
            info.OutOfPath = arg.CarSessionArg.OutOfPath;
            info.X = arg.CarSessionArg.X;
            info.Y = arg.CarSessionArg.Y;
            switch (arg.ReasonArg)
            {
                case CarSessionStateChangeArgs.Reason.Connect:
                    info.Reason = GisSessionReason.Add;
                    break;
                case CarSessionStateChangeArgs.Reason.Disconnect:
                    info.Reason = GisSessionReason.Remove;
                    break;
                case CarSessionStateChangeArgs.Reason.UpdateTemprary:
                    info.Reason = GisSessionReason.Update;
                    break;
            }
            info.Time = DateTime.Now;
            return info;
        }

        private void FlushCachedMsg(object sender, EventArgs e)
        {
            if (!bRunning) return;
            lock (this)
            {
                if (listCachedMsg.Count == 0)
                    return;
                byte[] data = DataContractFormatSerializer.Serialize(listCachedMsg, false);
                try
                {
                    using (SQLiteCommand cmd = (SQLiteCommand)conn.CreateCommand())
                    {
                        SQLiteParameter paramData = new SQLiteParameter("@data");
                        paramData.Value = data;
                        cmd.CommandText = string.Format("insert into csmmsg (time, data) values ({0}, @data)",
                            Ultility.TimeEncode(listCachedMsg[0].Time));
                        cmd.Parameters.Add(paramData);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (System.Exception)
                {
                	
                }
                
                listCachedMsg.Clear();
            }
        }
    }
   
    class HistoryCarSession
    {
        IDbConnection conn;
        const int MAX_DATA_PER_QUERY = 50;
        public HistoryCarSession(IDbConnection connection)
        {
            conn = connection;
        }
        public GisSessionInfo[] Query(DateTime tmStart, DateTime tmEnd)
        {
            if (tmStart != DateTime.MaxValue)
                return QueryHistory(tmStart, tmEnd);
            else
                return QueryImmdiate();
        }
        private GisSessionInfo[] QueryHistory(DateTime tmStart, DateTime tmEnd)
        {
            int start = Ultility.TimeEncode(tmStart);
            int end = Ultility.TimeEncode(tmEnd);
            List<GisSessionInfo> result = new List<GisSessionInfo>();
            byte[] buffer = new byte[1024 * 1024];
            using (IDbCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = string.Format("select data from csmmsg where time >= {0} and time < {1} order by time DESC limit 0,1",
                    start, end);
                using (var reader = cmd.ExecuteReader())
                {
                    int nDataNum = 0;
                    while (reader.Read())
                    {
                        if (nDataNum++ > MAX_DATA_PER_QUERY) break;
                        long dateLen = reader.GetBytes(0, 0, buffer, 0, buffer.Length);
                        GisSessionInfo[] resultTemp = DataContractFormatSerializer.Deserialize<GisSessionInfo[]>(buffer, (int)dateLen, false);
                        if (resultTemp == null) continue;
                        result.AddRange(resultTemp);
                    }
                }
            }
            return result.ToArray();
        }
        private GisSessionInfo[] QueryImmdiate()
        {
            List<GisSessionInfo> result = new List<GisSessionInfo>();
            byte[] buffer = new byte[1024 * 1024];
            using (IDbCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = string.Format("select * from csmmsg order by time DESC limit 0,1");
                using (var reader = cmd.ExecuteReader())
                {
                    int nDataNum = 0;
                    while (reader.Read())
                    {
                        int itm = reader.GetInt32(0);
                        DateTime tm = Ultility.TimeDecode(itm);
                        if ((DateTime.Now - tm).Duration().Minutes > 1) break;
                        if (nDataNum++ > MAX_DATA_PER_QUERY) break;
                        long dateLen = reader.GetBytes(1, 0, buffer, 0, buffer.Length);
                        GisSessionInfo[] resultTemp = DataContractFormatSerializer.Deserialize<GisSessionInfo[]>(buffer, (int)dateLen, false);
                        if (resultTemp == null) continue;
                        result.AddRange(resultTemp);
                    }
                }
            }
            return result.ToArray();
        }
    }
}
