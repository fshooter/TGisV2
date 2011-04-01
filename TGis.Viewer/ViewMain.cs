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
    partial class ViewMain : Form
    {
        private MainToolController controller;
        private MainToolModel model;
        public ViewMain(MainToolController controller, MainToolModel model)
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
            MenuItemPath.DropDownItems.Clear();
            ToolStripItem itemNew = new ToolStripMenuItem();
            itemNew.Text = "新建路径";
            itemNew.Click += new EventHandler((s, ee) => { controller.CreateNewPath(); });
            this.MenuItemPath.DropDownItems.Add(itemNew);
            this.MenuItemPath.DropDownItems.Add(new ToolStripSeparator());
            foreach (Path p in GisGlobal.GPathMgr.Paths)
            {
                ToolStripItem item = new ToolStripMenuItem();
                item.Tag = p.Id;
                item.Text = p.Name;
                item.Click += new EventHandler((s, eee) => { ToolStripItem t = s as ToolStripItem; controller.ModifyPath((int)t.Tag); });
                this.MenuItemPath.DropDownItems.Add(item);
            }
        }
        private void ReloadCarMenu(object sender, EventArgs e)
        {
            MenuItemCar.DropDownItems.Clear();
            ToolStripItem itemNew = new ToolStripMenuItem();
            itemNew.Text = "新建车辆";
            itemNew.Click += new EventHandler((s, ee) => { controller.CreateNewCar(); });
            this.MenuItemCar.DropDownItems.Add(itemNew);
            this.MenuItemCar.DropDownItems.Add(new ToolStripSeparator());
            foreach (Car p in GisGlobal.GCarMgr.Cars)
            {
                ToolStripItem item = new ToolStripMenuItem();
                item.Tag = p.Id;
                item.Text = p.Name;
                item.Click += new EventHandler((s, eee) => { ToolStripItem t = s as ToolStripItem; controller.ModifyCar((int)t.Tag); });
                this.MenuItemCar.DropDownItems.Add(item);
            }
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
        }
        private void PathState_Change(object sender, PathStateChangeArgs arg)
        {
            this.BeginInvoke(new EventHandler(ReloadPathMenu), new object[] { this, null });
        }
        private void CarState_Change(object sender, CarStateChangeArgs arg)
        {
            this.BeginInvoke(new EventHandler(ReloadCarMenu), new object[] { this, null });
        }
    }
}
