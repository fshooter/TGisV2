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
            if (!GisGlobal.GetCurrentIsAdmin())
            {
                this.barButtonDelete.Enabled = false;
                this.textEditSerial.Enabled = false;
                this.btnDelete.Visible = false;
                this.btnDelete.Enabled = false;
            }
            this.barStaticInfo.Caption = "您可以点击菜单上的编辑按钮编辑车辆配置";
            GisCarInfo c;
            GisCarDetail detail;
            if (!GisGlobal.GCarMgr.TryGetCar(carId, out c)
                || !GisServiceWrapper.Instance.QueryCarDetail(out detail, carId))
            {
                MessageBox.Show("传入车辆ID错误");
                this.barButtonOk.Enabled = false;
                return;
            }
            
            this.textEditName.EditValue = c.Name;
            this.textEditSerial.EditValue = c.SerialNum;
            this.textEditChepai.EditValue = detail.Chepai;
            this.memoEditComment.Text = detail.Comment;
            this.comboPath.Properties.Items.Add("自由模式");
            this.comboPath.SelectedItem = "自由模式";
            foreach (GisPathInfo p in GisGlobal.GPathMgr.Paths)
            {
                this.comboPath.Properties.Items.Add(p.Name);
                if (c.PathId == p.Id)
                    this.comboPath.SelectedItem = p.Name;
            }
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
            int selectedPathId = (selectedPath == null ? -1 : selectedPath.Id);
            GisCarInfo newcarinfo = new GisCarInfo();
            newcarinfo.Id = carId;
            newcarinfo.Name = this.textEditName.Text;
            newcarinfo.SerialNum = this.textEditSerial.Text;
            newcarinfo.PathId = selectedPathId;
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
            GisCarDetail detail = new GisCarDetail();
            detail.Id = carId;
            detail.Chepai = this.textEditChepai.Text;
            detail.Comment = this.memoEditComment.Text;
            if (GisServiceWrapper.Instance.UpdateCarDetail(detail))
            {
                MessageBox.Show("保存成功");
                NaviHelper.NaviToWelcome();
            }
            else
                MessageBox.Show("保存失败，请检查您的网络连接或重试");
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

        private void ViewModifyCar2_Shown(object sender, EventArgs e)
        {
            //var newp = NaviHelper.FormMain.ribbon.MergedPages["编辑"];
            //NaviHelper.FormMain.ribbon.SelectedPage =
            //    newp;
        }
    }
}