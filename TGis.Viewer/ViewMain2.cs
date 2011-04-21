using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraBars;
using TGis.Viewer.TGisRemote;
using DevExpress.XtraEditors;

namespace TGis.Viewer
{
    partial class ViewMain2 : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private MainToolController controller;
        private MainToolModel model;
        public ViewMain2(MainToolController controller, MainToolModel model)
        {
            this.controller = controller;
            this.model = model;
            InitializeComponent();
        }
        public void ResetActiveMenu()
        {
            this.ribbon.SelectedPage = this.ribbonPageMode;
        }
        private void 即时模式ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            controller.ImmediateMode();
        }

        private void ReloadPathMenu(object sender, EventArgs e)
        {
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).BeginInit();
            this.SuspendLayout();
            IList<BarItem> barItems = new List<BarItem>();
            foreach (BarItemLink item in this.ribbonPageGroupAllPaths.ItemLinks)
            {
                barItems.Add(item.Item);
            }
            foreach (BarItem item in barItems)
                this.ribbon.Items.Remove(item);
            this.ribbonPageGroupAllPaths.ItemLinks.Clear();
            foreach (GisPathInfo p in GisGlobal.GPathMgr.Paths)
            {
                var btnNew = this.ribbon.Items.CreateButton(p.Name);
                btnNew.Tag = p.Id;
                btnNew.ItemClick += new ItemClickEventHandler(PathMenu_Click);
                this.ribbonPageGroupAllPaths.ItemLinks.Add(btnNew);
            }
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).EndInit();
            this.ResumeLayout(false);
        }
        private void PathMenu_Click(object sender, ItemClickEventArgs e)
        {
            controller.ModifyPath((int)e.Item.Tag);
        }
        private void ReloadCarMenu(object sender, EventArgs e)
        {
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).BeginInit();
            this.SuspendLayout();
            IList<BarItem> barItems = new List<BarItem>();
            foreach (BarItemLink item in this.ribbonPageGroupAllCars.ItemLinks)
            {
                barItems.Add(item.Item);
            }
            foreach (BarItem item in barItems)
                this.ribbon.Items.Remove(item);
            this.ribbonPageGroupAllCars.ItemLinks.Clear();
            foreach (GisCarInfo c in GisGlobal.GCarMgr.Cars)
            {
                var btnNew = this.ribbon.Items.CreateButton(c.Name);
                btnNew.Tag = c.Id;
                btnNew.ItemClick += new ItemClickEventHandler(CarMenu_Click);
                this.ribbonPageGroupAllCars.ItemLinks.Add(btnNew);
            }
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).EndInit();
            this.ResumeLayout(false);
        }
        private void CarMenu_Click(object sender, ItemClickEventArgs e)
        {
            controller.ModifyCar((int)e.Item.Tag);
        }
        private void ViewMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            GisGlobal.GPathMgr.OnPathStateChanged -= new EventHandler(PathState_Change);
            GisGlobal.GCarMgr.OnCarStateChanged -= new EventHandler(CarState_Change);

            NaviHelper.FormStartup.Close();
        }

        private void ViewMain_Load(object sender, EventArgs e)
        {
            if (!GisGlobal.GetCurrentIsAdmin())
            {
                this.barButtonNewCar.Enabled = false;
            }
            ReloadPathMenu(this, null);
            ReloadCarMenu(this, null);
            ReloadAllMaps(this, e);
            GisGlobal.GPathMgr.OnPathStateChanged += new EventHandler(PathState_Change);
            GisGlobal.GCarMgr.OnCarStateChanged += new EventHandler(CarState_Change);
            this.ribbon.SelectedPage = ribbonPageMode;
            NaviHelper.NaviToWelcome();
        }
        private void PathState_Change(object sender, EventArgs arg)
        {
            this.BeginInvoke(new EventHandler(ReloadPathMenu), new object[] { this, null });
        }
        private void CarState_Change(object sender, EventArgs arg)
        {
            this.BeginInvoke(new EventHandler(ReloadCarMenu), new object[] { this, null });
        }

        private void barBtnWelcomeMode_Click(object sender, ItemClickEventArgs e)
        {
            controller.WelcomeMode();
        }

        private void barBtnImmMode_ItemClick(object sender, ItemClickEventArgs e)
        {
            controller.ImmediateMode();
        }

        private void barBtnNewPath_ItemClick(object sender, ItemClickEventArgs e)
        {
            controller.CreateNewPath();
        }

        private void barButtonNewCar_ItemClick(object sender, ItemClickEventArgs e)
        {
            controller.CreateNewCar();
        }

        private void barbtnHistoryMode_ItemClick(object sender, ItemClickEventArgs e)
        {
            controller.HistoryMode();
        }

        private void barButtonEventsMode_ItemClick(object sender, ItemClickEventArgs e)
        {
            controller.EventsMode();
        }

        private void btnModifyPass_ItemClick(object sender, ItemClickEventArgs e)
        {
            if((this.editNewPass.EditValue == null)
                || (this.editNewPass.EditValue.ToString().Length == 0))
            {
                MessageBox.Show("密码不能为空");
                return;
            }
            controller.ModifyPass(this.editNewPass.EditValue.ToString());
            MessageBox.Show("密码修改成功");
        }
        private void ReloadAllMaps(object sender, EventArgs e)
        {
            var dictMap = GisGlobal.GetAllMaps();
            this.repositoryItemComboBox1.Items.Clear();
            foreach (var kv in dictMap)
            {
                this.repositoryItemComboBox1.Items.Add(kv.Key);
            }
        }
        private void btnChangeMap_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (this.editNewMap.EditValue != null)
            {
                GisGlobal.SetSelectedMapName(this.editNewMap.EditValue.ToString());
                MessageBox.Show("设置地图成功");
            }
        }
    }
}