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
    public delegate void MapLoadCompleteHandler(object sender);
    public partial class MapControl : UserControl
    {
        private delegate void DeleUpdateCar(int id, string name, double x, double y, bool bException, bool bShow);
        private delegate void DeleAddRemoveCar(int id);
        private delegate void DeleAddPath(int id, string name, double[] points);
        private delegate void DeleRemovePath(int id);
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
        public void AddCar(int id)
        {
            webBrowser.Document.InvokeScript("add_car", new object[] { id });
        }
        public void UpdateCar(int id, string name, double x, double y, bool bException, bool bShow)
        {
            webBrowser.Document.InvokeScript("update_car", new object[] { id, x, y, bException, bShow });
        }
        public void RemoveCar(int id)
        {
            webBrowser.Document.InvokeScript("remove_car", new object[] { id });
        }
        public void AsynAddPath(int id, string name, double[] points)
        {
            this.BeginInvoke(new DeleAddPath(AddPath), new object[] { id, name, points });
        }
        public void AsynRemovePath(int id)
        {
            this.BeginInvoke(new DeleRemovePath(RemovePath), new object[] { id });
        }
        public void AddPath(int id, string name, double[] points)
        {
            object[] points_c = new object[points.Length];
            for (int i = 0; i < points.Length; ++i)
                points_c[i] = points[i];
            var jsarr = Microsoft.JScript.GlobalObject.Array.ConstructArray(points_c);
            webBrowser.Document.InvokeScript("add_path", new object[] {id, name, jsarr });
        }
        public void RemovePath(int id)
        {
            webBrowser.Document.InvokeScript("remove_path", new object[] { id });
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
        public MapLoadCompleteHandler OnMapLoadCompleted;
        public void AsynAddCar(int id)
        {
            this.BeginInvoke(new DeleAddRemoveCar(AddCar), new object[] { id });
        }
        public void AsynUpdateCar(int id, string name, double x, double y, bool bException, bool bShow)
        {
            this.BeginInvoke(new DeleUpdateCar(UpdateCar), new object[] { id, name, x, y, bException, bShow });
        }
        public void AsynRemoveCar(int id)
        {
            this.BeginInvoke(new DeleAddRemoveCar(RemoveCar), new object[] { id });
        }
        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (OnMapLoadCompleted != null)
                OnMapLoadCompleted(this);
        }
    }
}
