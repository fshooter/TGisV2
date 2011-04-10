using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Data;
using TGis.Common;

namespace TGis.RemoteService
{
    class TaskSchduler
    {
        Timer timer;
        IDbConnection conn;
        string dbPath;
        public void Run(IDbConnection conn, string dbPath)
        {
            this.dbPath = dbPath;
            this.conn = conn;
            timer = new Timer();
            timer.Interval = 1000 * 3600; // 1小时
            timer.Elapsed += new ElapsedEventHandler(TimerHandler);
            timer.Start();
        }
        public void Stop()
        {
            timer.Stop();
        }
        private void TimerHandler(object sender, ElapsedEventArgs e)
        {
            DateTime tm = DateTime.Now;
            if (tm.Hour != 3) return;
            if (tm.DayOfWeek != DayOfWeek.Sunday)
                return;
            BackupDb();
            ClearDb();
        }
        private void BackupDb()
        {
            System.IO.File.Copy(dbPath, dbPath + ".weekly", true);
            if (DateTime.Now.Day == 1)
            {
                System.IO.File.Copy(dbPath, dbPath + ".monthly", true);
            }
        }
        private void ClearDb()
        {
            using (IDbCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = string.Format("delete from csmmsg where time < {0}",
                    Ultility.TimeEncode(DateTime.Now.Date.Subtract(new TimeSpan(100, 0, 0, 0))));
                cmd.ExecuteNonQuery();
            }
        }
    }
}
