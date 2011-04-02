using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TGis.Viewer
{
    public partial class ViewModifyCar : Form
    {
        private int carId;
        class PathComoboItem
        {
            string name;
            int pathId;

            public int PathId
            {
                get { return pathId; }
            }
            public override string ToString()
            {
                return name;
            }
            public PathComoboItem(string n, int id)
            {
                name = n;
                pathId = id;
            }
        }
        public ViewModifyCar(int cid)
        {
            carId = cid;
            InitializeComponent();
        }

        private void ViewModifyCar_Load(object sender, EventArgs e)
        {
            Car c;
            if (!GisGlobal.GCarMgr.TryGetCar(carId, out c))
            {
                MessageBox.Show("传入车辆ID错误");
                this.btnOk.Enabled = false;
                return;
            }
            this.textName.Text = c.Name;
            foreach (Path p in GisGlobal.GPathMgr.Paths)
            {
                PathComoboItem itm = new PathComoboItem(p.Name, p.Id);
                this.comboPath.Items.Add(itm);
                if (itm.PathId == c.PathId)
                    this.comboPath.SelectedItem = itm;
            }
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (comboPath.SelectedItem == null)
            {
                MessageBox.Show("请选择该车辆适用的路径");
                return;
            }
            Car newcarinfo = new Car(carId, this.textName.Text, ((PathComoboItem)comboPath.SelectedItem).PathId);
            bool bNameValid = true;
            foreach (Car c in GisGlobal.GCarMgr.Cars)
            {
                if ((c.Id != newcarinfo.Id) && (c.Name == newcarinfo.Name))
                {
                    bNameValid = false;
                    break;
                }
            }
            if (!bNameValid)
            {
                MessageBox.Show("名称重复");
                return;
            }
            GisGlobal.GCarMgr.UpdateCar(newcarinfo);
            NaviHelper.NaviToWelcome();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            NaviHelper.NaviToWelcome();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            GisGlobal.GCarMgr.RemoveCar(new Car(carId, "", -1));
            NaviHelper.NaviToWelcome();
        }
    }
}
