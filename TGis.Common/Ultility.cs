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
            return Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
        }
    }
}
