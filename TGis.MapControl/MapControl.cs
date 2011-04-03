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
        private IDictionary<int, string> dictMarkers = new Dictionary<int, string>();
        private IDictionary<int, string> dictPathIdentify = new Dictionary<int, string>();
        private delegate void DeleAddUpdateCar(int id, string name, double x, double y, bool bException);
        private delegate void DeleRemoveCar(int id);
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
        public void AddCar(int id, string name, double x, double y, bool bException)
        {
            if (dictMarkers.ContainsKey(id))
                return;
            string marker = (string)webBrowser.Document.InvokeScript("add_car", new object[] { name, x, y, bException });
            dictMarkers[id] = marker;
        }
        public void UpdateCar(int id, string name, double x, double y, bool bException)
        {
            string marker;
            if (!dictMarkers.TryGetValue(id, out marker))
                return;
            webBrowser.Document.InvokeScript("update_car", new object[] { marker, x, y, bException });
        }
        public void RemoveCar(int id)
        {
            string marker;
            if (!dictMarkers.TryGetValue(id, out marker))
                return;
            dictMarkers.Remove(id);
            webBrowser.Document.InvokeScript("remove_car", new object[] { marker });
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
            string pathIdentify = (string)webBrowser.Document.InvokeScript("add_path", new object[] { name, jsarr });
            dictPathIdentify[id] = pathIdentify;
        }
        public void RemovePath(int id)
        {
            string pathIdentify;
            if (!dictPathIdentify.TryGetValue(id, out pathIdentify))
                throw new ApplicationException("MapControl!UpdateCar error");
            dictPathIdentify.Remove(id);
            webBrowser.Document.InvokeScript("remove_path", new object[] { pathIdentify });
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
        public void AsynAddCar(int id, string name, double x, double y, bool bException)
        {
            this.BeginInvoke(new DeleAddUpdateCar(AddCar), new object[] { id, name, x, y, bException });
        }
        public void AsynUpdateCar(int id, string name, double x, double y, bool bException)
        {
            this.BeginInvoke(new DeleAddUpdateCar(UpdateCar), new object[] { id, name, x, y, bException });
        }
        public void AsynRemoveCar(int id)
        {
            this.BeginInvoke(new DeleRemoveCar(RemoveCar), new object[] { id });
        }
        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (OnMapLoadCompleted != null)
                OnMapLoadCompleted(this);
        }
    }
}
