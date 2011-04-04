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
using TGis.Viewer.TGisRemote;

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
            if (model.SessionMgr.ImmMode)
                this.ribbonPageControl.Visible = false;
            MapControl_Load(sender, e);
            TreeList_Load(sender, e);
            GridCar_Load(sender, e);
            ControlPanel_Load(sender, e);
        }
        private void ViewGisCar_FormClosing(object sender, FormClosingEventArgs e)
        {
            MapCotrol_Closeing();
            GridCar_Closing(sender, e);
            model.Stop();
        }
#region ControlPanel
        private void ControlPanel_Load(object sender, EventArgs e)
        {
            this.ControlPanel_Day.EditValue = model.SessionMgr.CurrentTime;
            this.ControlPanel_Time.EditValue = model.SessionMgr.CurrentTime;
            this.model.SessionMgr.OnBeginQuerySessionMsg += new EventHandler(ControlPanel_OnBeginQuerySessionMsg);
        }
        private void ControlPanel_BtnGo_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.model.SessionMgr.CurrentTime = ((DateTime)this.ControlPanel_Day.EditValue).Date
                + ((DateTime)this.ControlPanel_Time.EditValue).TimeOfDay;
        }
        private void ControlPanel_BtnSpeedOk_ItemClick(object sender, ItemClickEventArgs e)
        {
            object editvalue = ControlPanel_Speed.EditValue;
            if(editvalue == null) return;
            model.SessionMgr.Multiply = Convert.ToInt32(editvalue.ToString());
        }
        private void ControlPanel_OnBeginQuerySessionMsg(object sender, EventArgs e)
        {
            this.barStaticUpdateTime.Caption = string.Format("更新时间: {0}",
                model.SessionMgr.CurrentTime);
        }
        private void ControlPanel_Close(object sender, EventArgs e)
        {
            this.model.SessionMgr.OnBeginQuerySessionMsg -= new EventHandler(ControlPanel_OnBeginQuerySessionMsg);
        }
#endregion

#region MapControl
        private void MapControl_Load(object sender, EventArgs e)
        {
            mapControl1.Navigate(Ultility.GetAppDir() + @"\map\map.html");
            mapControl1.OnMapLoadCompleted += new MapLoadCompleteHandler(AsynInitMapFirstTime);
            
        }
        private void AsynInitMapFirstTime(object map)
        {
            this.BeginInvoke(new EventHandler(InitMapFirstTime), new object[] { this, null });
        }
        private void InitMapFirstTime(object map, EventArgs e)
        {
            bInitComplete = true;
            foreach (GisCarInfo c in GisGlobal.GCarMgr.Cars)
            {
                mapControl1.AsynAddCar(c.Id);
            }
            model.SessionMgr.OnSessionMsgReceived += new CarSessionMsgHandler(MapCotrol_SessionMessageHandler);
            model.OnCarShowChanged += new EventHandler(MapControl_CarShowChang);
            model.OnPathShowChanged += new EventHandler(MapControl_PathShowChang);
        }
        private void MapCotrol_Closeing()
        {
            mapControl1.OnMapLoadCompleted -= new MapLoadCompleteHandler(AsynInitMapFirstTime);
            model.SessionMgr.OnSessionMsgReceived -= new CarSessionMsgHandler(MapCotrol_SessionMessageHandler);
            model.OnCarShowChanged -= new EventHandler(MapControl_CarShowChang);
            model.OnPathShowChanged -= new EventHandler(MapControl_PathShowChang);
            
        }
        private void MapCotrol_SessionMessageHandler(object sender, GisSessionInfo msg)
        {
            
            switch (msg.Reason)
            {
                case GisSessionReason.Update:
                    if(model.GetCarShow(msg.CarId))
                        mapControl1.UpdateCar(msg.CarId, "", msg.X, msg.Y, msg.OutOfPath, true);
                    break;
                case GisSessionReason.Remove:
                    mapControl1.UpdateCar(msg.CarId, "", 0, 0, true, false);
                    break;
            }
        }
        private void MapControl_CarShowChang(object sender, EventArgs e)
        {
            foreach (var c in GisGlobal.GCarMgr.Cars)
            {
                if (!model.GetCarShow(c.Id))
                    mapControl1.UpdateCar(c.Id, "", 0, 0, true, false);
            }
        }
        private void MapControl_PathShowChang(object sender, EventArgs e)
        {
            foreach (var p in GisGlobal.GPathMgr.Paths)
            {
                if (model.GetPathShow(p.Id))
                    mapControl1.AddPath(p.Id, p.Name, p.Points);
                else
                    mapControl1.RemovePath(p.Id);
            }
        }
#endregion
       
#region TreeList
        private void TreeList_Load(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.CheckEdit[] checks =
                new DevExpress.XtraEditors.CheckEdit[GisGlobal.GCarMgr.Cars.Length];
            int i = 0;
            foreach (GisCarInfo c in GisGlobal.GCarMgr.Cars)
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
            foreach (GisPathInfo p in GisGlobal.GPathMgr.Paths)
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
                    model.UserMakeCarShow((int)check.Tag, true);
                    break;
                case CheckState.Unchecked:
                    model.UserMakeCarShow((int)check.Tag, false);
                    break;
            }
        }
        private void TreeListPathCheck_Changed(object sender, EventArgs arg)
        {
            DevExpress.XtraEditors.CheckEdit check = (DevExpress.XtraEditors.CheckEdit)sender;
            switch (check.CheckState)
            {
                case CheckState.Checked:
                    model.UserMakePathShow((int)check.Tag, true);
                    break;
                case CheckState.Unchecked:
                    model.UserMakePathShow((int)check.Tag, false);
                    break;
            }
        }
#endregion

#region CarStatusGrid
        private void GridCar_Load(object sender, EventArgs e)
        {
            foreach (GisCarInfo c in GisGlobal.GCarMgr.Cars)
            {
                int newRowId = dataGridView1.Rows.Add();
                DataGridViewRow row = dataGridView1.Rows[newRowId];
                row.Cells[0].Value = c.Name;
                row.Cells[1].Value = 0;
                row.Cells[2].Value = 0;
                row.Cells[3].Value = "正转";
                row.Cells[4].Value = "正常";
                row.Cells[5].Value = "掉线";
                row.Tag = c.Id;
            }
            model.SessionMgr.OnSessionMsgReceived += new CarSessionMsgHandler(GridCar_SessionMessageHandler);
        }
        private void GridCar_Closing(object sender, EventArgs e)
        {
            model.SessionMgr.OnSessionMsgReceived -= new CarSessionMsgHandler(GridCar_SessionMessageHandler);
        }
        private void GridCar_SessionMessageHandler(object sender, GisSessionInfo msg)
        {
            switch (msg.Reason)
            {
                case GisSessionReason.Update:
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if ((int)row.Tag == msg.CarId)
                        {
                            row.Cells[1].Value = msg.X;
                            row.Cells[2].Value = msg.Y;
                            row.Cells[3].Value = msg.RoolDirection ? "正转" : "反转";
                            row.Cells[4].Value = msg.OutOfPath ? "正常路径" : "异常路径";
                            row.Cells[5].Value = msg.Alive ? "连线" : "掉线";
                            break;
                        }
                    }
                    break;
                case GisSessionReason.Remove:
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if ((int)row.Tag == msg.CarId)
                        {
                            row.Cells[1].Value = msg.X;
                            row.Cells[2].Value = msg.Y;
                            row.Cells[3].Value = msg.RoolDirection ? "正转" : "反转";
                            row.Cells[4].Value = msg.OutOfPath ? "正常路径" : "异常路径";
                            row.Cells[5].Value = "掉线";
                            break;
                        }
                    }
                    break;
            }
        }

#endregion
        
        
    }
}