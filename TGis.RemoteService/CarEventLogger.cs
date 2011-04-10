using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TGis.RemoteContract;
using TGis.Common;
using System.Data;
using System.Data.SQLite;

namespace TGis.RemoteService
{
    struct OldCarState
    {
        public bool OutOfPath;
        public bool RollBackward;
    }
    class CarEventLogger
    {
        CarSessionMgr csm;
        IDbConnection conn;
        IDictionary<int, OldCarState> oldStates = new Dictionary<int, OldCarState>();
        public CarEventLogger(IDbConnection connection, CarSessionMgr csm)
        {
            this.csm = csm;
            conn = connection;
            using (IDbCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"create table if not exists events 
                    (time INTEGER, data BLOB)";
                cmd.ExecuteNonQuery();
                cmd.CommandText = @"create index if not exists events_index 
                    on events (time)";
                cmd.ExecuteNonQuery();
            }
            
        }
        public void Run()
        {
            this.csm.OnCarSessionStateChanged += new CarSessionStateChangeHandler(CarSessionStateChange_Handler);
        }
        public void Stop()
        {
            this.csm.OnCarSessionStateChanged -= new CarSessionStateChangeHandler(CarSessionStateChange_Handler);
        }
        private void CarSessionStateChange_Handler(object sender, CarSessionStateChangeArgs args)
        {
            OldCarState oldState;
            switch(args.ReasonArg)
            {
                case CarSessionStateChangeArgs.Reason.Add:
                    OldCarState state;
                    state.OutOfPath = false;
                    state.RollBackward = false;
                    oldStates[args.CarSessionArg.CarInstance.Id] = state;
                    break;
                case CarSessionStateChangeArgs.Reason.Remove:
                    oldStates.Remove(args.CarSessionArg.CarInstance.Id);
                    break;
                case CarSessionStateChangeArgs.Reason.Connect:
                    LogEvent(GisEventType.Connect, args);
                    break;
                case CarSessionStateChangeArgs.Reason.Disconnect:
                    LogEvent(GisEventType.DisConnect, args);
                    break;
                case CarSessionStateChangeArgs.Reason.UpdateTemprary:
                    if (!oldStates.TryGetValue(args.CarSessionArg.CarInstance.Id, out oldState))
                        break;
                    if (!oldState.OutOfPath && args.CarSessionArg.OutOfPath)
                        LogEvent(GisEventType.OutOfPath, args);
                    if (!oldState.RollBackward && (args.CarSessionArg.RollDirection == CarRollDirection.Backward))
                        LogEvent(GisEventType.RollBackward, args);
                    break;
            }
        }
        private void LogEvent(GisEventType type, CarSessionStateChangeArgs arg)
        {
            GisEventInfo info = new GisEventInfo();
            info.Type = type;
            info.CarId = arg.CarSessionArg.CarInstance.Id;
            info.X = arg.CarSessionArg.X;
            info.Y = arg.CarSessionArg.Y;
            info.Time = arg.CarSessionArg.LastUpdateTime;
            byte[] data = DataContractFormatSerializer.Serialize(info, false);
            try
            {
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    SQLiteParameter paramData = new SQLiteParameter("@data");
                    paramData.Value = data;
                    cmd.CommandText = string.Format("insert into events (time, data) values ({0}, @data)",
                        Ultility.TimeEncode(info.Time));
                    cmd.Parameters.Add(paramData);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (System.Exception)
            {
            	
            }
            
        }
    }

    class CarEventQueryer
    {
        const int MAX_DATA_PER_QUERY = 50;
        IDbConnection conn;
        public CarEventQueryer(IDbConnection connection)
        {
            conn = connection;
        }
        public GisEventInfo[] Query(DateTime tmStart, DateTime tmEnd, out bool bTobeContinue)
        {
            bTobeContinue = false;
            int start = Ultility.TimeEncode(tmStart);
            int end = Ultility.TimeEncode(tmEnd);
            List<GisEventInfo> result = new List<GisEventInfo>();
            byte[] buffer = new byte[1024 * 1024];
            using (IDbCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = string.Format("select data from events where time >= {0} and time < {1} ORDER BY time ASC",
                    start, end);
                using (var reader = cmd.ExecuteReader())
                {
                    int nDataNum = 0;
                    while (reader.Read())
                    {
                        if (nDataNum++ > MAX_DATA_PER_QUERY)
                        {
                            bTobeContinue = true;
                            break;
                        }
                        long dateLen = reader.GetBytes(0, 0, buffer, 0, buffer.Length);
                        GisEventInfo resultTemp = DataContractFormatSerializer.Deserialize<GisEventInfo>(buffer, (int)dateLen, false);
                        if (resultTemp == null) continue;
                        result.Add(resultTemp);
                    }
                }
            }
            return result.ToArray();
        }
    }
}
