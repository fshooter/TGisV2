using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TGis.MapControl
{
    public partial class MapControl : UserControl
    {
        public MapControl()
        {
            InitializeComponent();
        }

        private void MapControl_Load(object sender, EventArgs e)
        {
            
        }

        public void Navigate(string path)
        {
            webBrowser.Navigate(path);
        }
        public void AddCar(string name, double x, double y, bool bException)
        {
            webBrowser.Document.InvokeScript("add_car", new object[] { name, x, y, bException });
        }
        public void ClearAllCars()
        {
            webBrowser.Document.InvokeScript("clear_all_cars");
        }
        public void AddPath(double[] points)
        {
            object[] points_c = new object[points.Length];
            for (int i = 0; i < points.Length; ++i)
                points_c[i] = points[i];
            var jsarr = Microsoft.JScript.GlobalObject.Array.ConstructArray(points_c);
            webBrowser.Document.InvokeScript("add_path", new object[] { jsarr });
        }
        public void BeginDrawPath()
        {
            webBrowser.Document.InvokeScript("begin_draw_path");
        }
        public double[] EndDrawPath()
        {
            string r = webBrowser.Document.InvokeScript("end_draw_path") as string;
            if (r == null)
                return null;
            string[] points_str = r.Split(',');
            double[] result = new double[points_str.Length];
            for (int i = 0; i < result.Length; ++i)
                result[i] = Convert.ToDouble(points_str[i]);
            return result;
        }

        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }
    }
}
