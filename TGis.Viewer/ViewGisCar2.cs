using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraBars;
using TGis.Common;
using TGis.MapControl;

namespace TGis.Viewer
{
    partial class ViewGisCar2 : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private GisCarModel model;
        private GisCarController controller;
        private bool bInitComplete = false;
        public ViewGisCar2(GisCarModel model, GisCarController controller)
        {
            this.model = model;
            this.controller = controller;

            InitializeComponent();
        }

        private void ViewGisCar_Load(object sender, EventArgs e)
        {
            MapControl_Load(sender, e);
            TreeList_Load(sender, e);
            GridCar_Load(sender, e); ;
            
        }
        

#region MapControl
        private void MapControl_Load(object sender, EventArgs e)
        {
            mapControl1.Navigate(Ultility.GetAppDir() + @"\map\map.html");
            mapControl1.OnMapLoadCompleted += new MapLoadCompleteHandler(AsynInitMapFirstTime);
            model.OnCarShowableChanged += new CarShowableMsgHandler(CarShowableChange_Map);
            model.OnPathShowableChanged += new PathShowableMsgHandler(PathShowableChange_Map);
        }
        private void CarShowableChange_Map(object sender, GisCarModelReasonCar reason, int id, string name,
            double x, double y, bool bException)
        {
            if (!bInitComplete) return;
            switch (reason)
            {
                case GisCarModelReasonCar.Add:
                    mapControl1.AsynAddCar(id, name, x, y, bException);
                    break;
                case GisCarModelReasonCar.Update:
                    mapControl1.AsynUpdateCar(id, name, x, y, bException);
                    break;
                case GisCarModelReasonCar.Remove:
                    mapControl1.AsynRemoveCar(id);
                    break;
            }
        }
        private void PathShowableChange_Map(object sender, GisCarModelReasonPath reason, int id)
        {
            Path path;
            if (!GisGlobal.GPathMgr.TryGetPath(id, out path))
                return;
            switch (reason)
            {
                case GisCarModelReasonPath.Add:
                    mapControl1.AsynAddPath(id, path.Name, path.PathPolygon.Points);
                    break;
                case GisCarModelReasonPath.Remove:
                    mapControl1.AsynRemovePath(id);
                    break;
            }   
        }
        private void AsynInitMapFirstTime(object map)
        {
            this.BeginInvoke(new EventHandler(InitMapFirstTime), new object[] { this, null });
        }
        private void InitMapFirstTime(object map, EventArgs e)
        {
            bInitComplete = true;
            model.InitCarStateFirstTime();
        }
        private void MapCotrol_Closeing()
        {
            mapControl1.OnMapLoadCompleted -= new MapLoadCompleteHandler(AsynInitMapFirstTime);
            model.OnCarShowableChanged -= new CarShowableMsgHandler(CarShowableChange_Map);
            model.OnPathShowableChanged -= new PathShowableMsgHandler(PathShowableChange_Map);
        }
#endregion
       
#region TreeList
        private void TreeList_Load(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.CheckEdit[] checks =
                new DevExpress.XtraEditors.CheckEdit[GisGlobal.GCarMgr.Cars.Length];
            int i = 0;
            foreach (Car c in GisGlobal.GCarMgr.Cars)
            {
                DevExpress.XtraEditors.CheckEdit check = new DevExpress.XtraEditors.CheckEdit();
                
                check.Text = c.Name;
                checks[i++] = check;
                check.Tag = c.Id;
                check.CheckStateChanged += new EventHandler(TreeListCarCheck_Changed);
                check.Size = check.CalcBestSize();
            }
            this.flowLayoutPanel1.Controls.AddRange(checks);

            DevExpress.XtraEditors.CheckEdit[] checksPath =
                new DevExpress.XtraEditors.CheckEdit[GisGlobal.GPathMgr.Paths.Length];
            i = 0;
            foreach (Path p in GisGlobal.GPathMgr.Paths)
            {
                DevExpress.XtraEditors.CheckEdit check = new DevExpress.XtraEditors.CheckEdit();
                check.Text = p.Name;
                checksPath[i++] = check;
                check.Tag = p.Id;
                check.CheckStateChanged += new EventHandler(TreeListPathCheck_Changed);
                check.Size = check.CalcBestSize();
            }
            this.flowLayoutPanel2.Controls.AddRange(checksPath);
        }
        private void TreeListCarCheck_Changed(object sender, EventArgs arg)
        {
            DevExpress.XtraEditors.CheckEdit check = (DevExpress.XtraEditors.CheckEdit)sender;
            switch (check.CheckState)
            {
                case CheckState.Checked:
                    model.UserMakeCarShowable((int)check.Tag, true);
                    break;
                case CheckState.Unchecked:
                    model.UserMakeCarShowable((int)check.Tag, false);
                    break;
            }
        }
        private void TreeListPathCheck_Changed(object sender, EventArgs arg)
        {
            DevExpress.XtraEditors.CheckEdit check = (DevExpress.XtraEditors.CheckEdit)sender;
            switch (check.CheckState)
            {
                case CheckState.Checked:
                    model.UserMakePathShowable((int)check.Tag, true);
                    break;
                case CheckState.Unchecked:
                    model.UserMakePathShowable((int)check.Tag, false);
                    break;
            }
        }
#endregion

#region CarStatusGrid
        private void GridCar_Load(object sender, EventArgs e)
        {
            //dataGridView1.SuspendLayout();
            model.CsMgr.EnumCarSession(GridCar_OnCarSession);
            //dataGridView1.ResumeLayout(true);
            //dataGridView1.Invalidate();
            this.model.CsMgr.OnCarSessionStateChanged += new CarSessionStateChangeHandler(GridCar_AsynCarSessionState_Change);
        }
        private void GridCar_OnCarSession(CarSession cs)
        {
            int id = dataGridView1.Rows.Add();
            GridCar_ModifyRow(id, cs);
        }
        private void GridCar_ModifyRow(int rowid, CarSession cs)
        {
            DataGridViewRow row = dataGridView1.Rows[rowid];
            row.Tag = cs.CarInstance.Id;
            row.Cells[0].Value = cs.CarInstance.Name;
            row.Cells[1].Value = cs.X;
            row.Cells[2].Value = cs.Y;
            row.Cells[3].Value = cs.RollDirection == CarRollDirection.Forward ? "正转" : "反转";
            row.Cells[4].Value = (!cs.OutOfPath) ? "正常" : "异常";
            row.Cells[5].Value = cs.Alive ? "连线" : "掉线";
        }
        private void GridCar_AsynCarSessionState_Change(object sender, CarSessionStateChangeArgs arg)
        {
            this.BeginInvoke(new CarSessionStateChangeHandler(GridCar_CarSessionState_Change),
                new object[] { sender, arg });
        }
        private void GridCar_CarSessionState_Change(object sender, CarSessionStateChangeArgs arg)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Tag == null) continue;
                if ((int)row.Tag == arg.CarSessionArg.CarInstance.Id)
                {
                    GridCar_ModifyRow(row.Index, arg.CarSessionArg);
                    break;
                }
            }
        }
        private void GridCar_Closing(object sender, EventArgs e)
        {
            this.model.CsMgr.OnCarSessionStateChanged -= new CarSessionStateChangeHandler(GridCar_AsynCarSessionState_Change);
        }
#endregion
        
        private void TableControlSessionState_Change(object sender, CarSessionStateChangeArgs arg)
        {
            switch (arg.ReasonArg)
            {
                case CarSessionStateChangeArgs.Reason.Add:
                case CarSessionStateChangeArgs.Reason.Remove:
                case CarSessionStateChangeArgs.Reason.Update:
                    break;
                case CarSessionStateChangeArgs.Reason.UpdateTemprary:
                case CarSessionStateChangeArgs.Reason.Connect:
                case CarSessionStateChangeArgs.Reason.Disconnect:
                    break;
            }
        }
        

        private void ViewGisCar_FormClosing(object sender, FormClosingEventArgs e)
        {
            MapCotrol_Closeing();
            GridCar_Closing(sender, e);
            model.Stop();
        }
    }
}