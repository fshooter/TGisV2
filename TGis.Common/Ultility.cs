using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TGis.Common
{
    public class Ultility
    {
        public static string GetAppDir()
        {
            return System.Windows.Forms.Application.StartupPath;
        }
        public static string GetDataDir()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\TGis";
        }

        static DateTime tmMin = new DateTime(2000, 1, 1);
        public static int TimeEncode(DateTime tm)
        {
            if (tm <= tmMin)
                throw new ApplicationException("TinyGis时间/日期错误,溢出");
            return (int)(tm - tmMin).TotalSeconds;

        }
        public static DateTime TimeDecode(int v)
        {
            if (v < 0)
                throw new ApplicationException("TinyGis时间/日期错误,溢出");
            return tmMin.AddSeconds(v);
        }
    }
}
