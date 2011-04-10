using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraBars;
using TGis.Viewer.TGisRemote;

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
            this.barStaticInfo.Caption = "您可以点击菜单上的编辑按钮编辑车辆配置";
            GisCarInfo c;
            if (!GisGlobal.GCarMgr.TryGetCar(carId, out c))
            {
                MessageBox.Show("传入车辆ID错误");
                this.barButtonOk.Enabled = false;
                return;
            }
            this.textEditName.EditValue = c.Name;
            foreach (GisPathInfo p in GisGlobal.GPathMgr.Paths)
            {
                this.comboPath.Properties.Items.Add(p.Name);
                if (c.PathId == p.Id)
                    this.comboPath.SelectedItem = p.Name;
            }
            this.ribbon.SelectedPage = ribbonPage1;
        }

        private void barButtonOk_ItemClick(object sender, ItemClickEventArgs e)
        {
            btnSave_Click(sender, e);
        }

        private void barButtonCancel_ItemClick(object sender, ItemClickEventArgs e)
        {
            btnCancel_Click(sender, e);
        }

        private void barButtonDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            btnDelete_Click(sender, e);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (comboPath.SelectedItem == null)
            {
                MessageBox.Show("请选择该车辆适用的路径");
                return;
            }
            string pathName = (string)comboPath.SelectedItem;
            GisPathInfo selectedPath = null;
            foreach (var p in GisGlobal.GPathMgr.Paths)
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
            GisCarInfo newcarinfo = new GisCarInfo();
            newcarinfo.Id = carId;
            newcarinfo.Name = this.textEditName.Text;
            newcarinfo.PathId = selectedPath.Id;
            bool bNameValid = true;
            foreach (var c in GisGlobal.GCarMgr.Cars)
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
            try
            {
                GisGlobal.GCarMgr.UpdateCar(newcarinfo);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("更新车辆信息失败");
            }
            
            NaviHelper.NaviToWelcome();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                GisGlobal.GCarMgr.RemoveCar(carId);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("更新车辆信息失败");
            }
            
            NaviHelper.NaviToWelcome();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            NaviHelper.NaviToWelcome();
        }
    }
}