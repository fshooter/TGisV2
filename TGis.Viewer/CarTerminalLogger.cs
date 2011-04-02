using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TGis.Common;
using System.Timers;
using System.Data;

namespace TGis.Viewer
{
    class CarTernimalStateArgSerialHelper
    {
        public static string Encode(CarTernimalStateArg arg)
        {
            return string.Format("{0}, {1}, {2}, {3}, {4}",
                Ultility.TimeEncode(arg.Time), arg.PhoneNum, arg.X, arg.Y, arg.RollDirection);
        }
        public static CarTernimalStateArg Decode(string data)
        {
            string[] msgs = data.Split(',');
            if (msgs.Length != 5)
                throw new ApplicationException("Format Error");
            CarTernimalStateArg r = new CarTernimalStateArg;
            r.Time = Ultility.TimeDecode(Convert.ToInt32(msgs[0]));
            r.PhoneNum = msgs[1];
            r.X = Convert.ToInt32(msgs[2]);
            r.Y = Convert.ToInt32(msgs[3]);
            CarRollDirection direction;
            if (msgs[4] == "Forward")
                direction = CarRollDirection.Forward;
            else if (msgs[4] == "Backward")
                direction = CarRollDirection.Backward;
            else
                throw new ApplicationException("Format Error");
            r.RollDirection = direction;
            return r;
        }
    }
    class CarTerminalLogger
    {
        ICarTerminalAbility terminal;
        IList<CarTernimalStateArg> listCachedMsg = new List<CarTernimalStateArg>();
        Timer timer = new Timer();
        IDbConnection conn;
        bool bRunning = false;
        public CarTerminalLogger(ICarTerminalAbility terminal, IDbConnection connection)
        {
            this.terminal = terminal;
            conn = connection;
            using (IDbCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"create table if not exists msgs 
                    (time INTEGER, data TEXT)";
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
            terminal.OnCarStateChanged += new CarTerminalStateChangeHandler(TernimalMsgHandler);
        }
        public void Stop()
        {
            bRunning = false;
            terminal.OnCarStateChanged -= new CarTerminalStateChangeHandler(TernimalMsgHandler);
            timer.Stop();
        }

        private CarProcResult TernimalMsgHandler(object sender, CarTernimalStateArg arg)
        {
            if (!bRunning) return CarProcResult.Ok;
            lock (this)
            {
                listCachedMsg.Add(arg);
            }
            return CarProcResult.Ok;
        }

        private void FlushCachedMsg(object sender, EventArgs e)
        {
            if (!bRunning) return;
            lock (this)
            {
                if (listCachedMsg.Count == 0)
                    return;
                string data = "";
                foreach (CarTernimalStateArg msg in listCachedMsg)
                {
                    string dataSingle = CarTernimalStateArgSerialHelper.Encode(msg);
                    if (data.Length != 0)
                        data += '|';
                    data += dataSingle;
                }
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("insert into msgs (time, data) values ({0}, '{1}')",
                        Ultility.TimeEncode(listCachedMsg[0].Time), data);
                    cmd.ExecuteNonQuery();
                }
                listCachedMsg.Clear();
            }
        }
    }
}
