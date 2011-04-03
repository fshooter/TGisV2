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
            foreach (Path p in GisGlobal.GPathMgr.Paths)
            {
                var btnNew = this.ribbon.Items.CreateButton(p.Name);
                this.ribbonPageGroupAllPaths.ItemLinks.Add(btnNew);
            }
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).EndInit();
            this.ResumeLayout(false);
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
            foreach (Car c in GisGlobal.GCarMgr.Cars)
            {
                var btnNew = this.ribbon.Items.CreateButton(c.Name);
                this.ribbonPageGroupAllCars.ItemLinks.Add(btnNew);
            }
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).EndInit();
            this.ResumeLayout(false);
        }

        private void ViewMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            GisGlobal.GPathMgr.OnPathStateChanged -= new PathStateChangeHandler(PathState_Change);
            GisGlobal.GCarMgr.OnCarStateChanged -= new CarStateChangeHandler(CarState_Change);
        }

        private void ViewMain_Load(object sender, EventArgs e)
        {
            ReloadPathMenu(this, null);
            ReloadCarMenu(this, null);
            GisGlobal.GPathMgr.OnPathStateChanged += new PathStateChangeHandler(PathState_Change);
            GisGlobal.GCarMgr.OnCarStateChanged += new CarStateChangeHandler(CarState_Change);
            this.ribbon.SelectedPage = ribbonPageMode;
        }
        private void PathState_Change(object sender, PathStateChangeArgs arg)
        {
            this.BeginInvoke(new EventHandler(ReloadPathMenu), new object[] { this, null });
        }
        private void CarState_Change(object sender, CarStateChangeArgs arg)
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
    }
}