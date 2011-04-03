using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraBars;

namespace TGis.Viewer
{
    public partial class ViewModifyCar2 : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private int carId;
        public ViewModifyCar2(int cid)
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
                this.barButtonOk.Enabled = false;
                return;
            }
            this.textEditName.EditValue = c.Name;
            foreach (Path p in GisGlobal.GPathMgr.Paths)
            {
                this.comboPath.Properties.Items.Add(p.Name);
                if (c.PathId == p.Id)
                    this.comboPath.SelectedItem = p.Name;
            }
        }

        private void barButtonOk_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (comboPath.SelectedItem == null)
            {
                MessageBox.Show("请选择该车辆适用的路径");
                return;
            }
            string pathName = (string)comboPath.SelectedItem;
            Path selectedPath = null;
            foreach (Path p in GisGlobal.GPathMgr.Paths)
            {
                if (p.Name == pathName)
                {
                    selectedPath = p;
                    break;
                }
            }
            if (selectedPath == null)
            {
                MessageBox.Show("请选择该车辆适用的路径");
                return;
            }
            Car newcarinfo = new Car(carId, this.textEditName.Text, selectedPath.Id);
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

        private void barButtonCancel_ItemClick(object sender, ItemClickEventArgs e)
        {
            NaviHelper.NaviToWelcome();
        }

        private void barButtonDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            GisGlobal.GCarMgr.RemoveCar(new Car(carId, "", -1));
            NaviHelper.NaviToWelcome();
        }
    }
}