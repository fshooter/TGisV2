using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TGis.Common;

namespace TGis.Viewer
{
    public partial class ViewGisCar : Form
    {
        public ViewGisCar()
        {
            InitializeComponent();
        }

        private void ViewGisCar_Load(object sender, EventArgs e)
        {
            mapControl1.Navigate(Ultility.GetAppDir() + @"\map\map.html");
        }
    }
}
