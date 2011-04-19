using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraBars;
using TGis.Viewer.TGisRemote;
using TGis.Common;
using TGis.MapControl;

namespace TGis.Viewer
{
    partial class ViewEvents : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        EventsQueryModel model;
        public ViewEvents(EventsQueryModel model)
        {
            this.model = model;
            InitializeComponent();
        }

        private void ViewEvents_Load(object sender, EventArgs e)
        {
            this.barStaticInfo.Caption = "您可以点击菜单上的查询按钮查询指定时间的事件";
            List_Load(sender, e);
            MapControl_Load(sender, e);
            ReloadPreNextBtnState();
            barTimeInit();
        }
        private void ViewEvents_FormClosing(object sender, FormClosingEventArgs e)
        {
            ListUnload(sender, e);
            MapCotrol_Closeing();
        }
#region ListView
        private void List_Load(object sender, EventArgs e)
        {
            model.OnEventsChanged += new EventHandler(ReloadDatas);
        }
        private void ListUnload(object sender, EventArgs e)
        {
            model.OnEventsChanged -= new EventHandler(ReloadDatas);
        }
        private void ReloadDatas(object sender, EventArgs e)
        {
            dataGridView1.SuspendLayout();
            dataGridView1.Rows.Clear();
            foreach (var ei in model.Events)
            {
                GisCarInfo car;
                if(!GisGlobal.GCarMgr.TryGetCar(ei.CarId, out car))
                    continue;
                int newid = dataGridView1.Rows.Add();
                var row = dataGridView1.Rows[newid];
                row.Tag = ei;
                row.Cells[0].Value = ei.Time;
                row.Cells[1].Value = MapEventTypeToString(ei.Type);
                row.Cells[2].Value = car.Name;
                row.Cells[3].Value = "前往";
            }
            dataGridView1.ResumeLayout(true);
        }
        private string MapEventTypeToString(GisEventType t)
        {
            string r = "未知事件";
            switch (t)
            {
                case TGisRemote.GisEventType.Connect:
                    r = "连线";
                    break;
                case TGisRemote.GisEventType.DisConnect:
                    r = "掉线";
                    break;
                case TGisRemote.GisEventType.OutOfPath:
                    r = "路径异常";
                    break;
                case TGisRemote.GisEventType.RollBackward:
                    r = "反转";
                    break;
            }
            return r;
        }
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count < 1)
                return;
            var ei = dataGridView1.SelectedRows[0].Tag as GisEventInfo;
            if (ei == null) return;
            model.EventSelected = ei;
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 3) return;
            if (e.RowIndex == -1) return;
            GisEventInfo ei = this.model.EventSelected;
            if (ei == null) return;
            CarSessionMgr csm = new CarSessionMgr(false);
            csm.CurrentTime = ei.Time;
            GisCarModel modelcar = new GisCarModel(csm);
            GisCarController controller = new GisCarController();
            Form viewGisCar = new ViewGisCar2(modelcar, controller);
            csm.SynObject = viewGisCar;
            csm.Run(2000);
            NaviHelper.NaviTo(viewGisCar);
        }
#endregion

#region MapControl
        private int oldCarId = -1;
        private void MapControl_Load(object sender, EventArgs e)
        {
            mapControl.Navigate(GisGlobal.GetSelectedMapPath());
            mapControl.OnMapLoadCompleted += new MapLoadCompleteHandler(AsynInitMapFirstTime);

        }
        private void AsynInitMapFirstTime(object map)
        {
            this.BeginInvoke(new EventHandler(InitMapFirstTime), new object[] { this, null });
        }
        private void InitMapFirstTime(object map, EventArgs e)
        {
            model.OnEventSelected += new EventHandler(MapControl_SelectedCarChanged);
        }
        private void MapCotrol_Closeing()
        {
            model.OnEventSelected -= new EventHandler(MapControl_SelectedCarChanged);

        }
        private void MapControl_SelectedCarChanged(object sender, EventArgs e)
        {
            mapControl.RemoveCar(oldCarId);
            GisEventInfo ei = model.EventSelected;
            mapControl.AddCar(ei.CarId);
            mapControl.UpdateCar(ei.CarId, "", ei.X, ei.Y, true, true);
            mapControl.SetCenter(ei.X, ei.Y);
            oldCarId = ei.CarId;
        }
#endregion

#region PreAndNextBtn
        private void barBtnPrePage_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (model.CanQueryPre)
                model.QueryPre();
            ReloadPreNextBtnState();
        }
        private void barBtnNextPage_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (model.CanQueryNext)
                model.QueryNext();
            ReloadPreNextBtnState();
        }
        private void ReloadPreNextBtnState()
        {
            barBtnPrePage.Enabled = model.CanQueryPre;
            barBtnNextPage.Enabled = model.CanQueryNext;
        }
#endregion
        private void barTimeInit()
        {
            var zeroTime = new DateTime();
            this.barEditDataStart.EditValue = DateTime.Now.Date;
            this.barEditTimeStart.EditValue = zeroTime;
            this.barEditDateEnd.EditValue = DateTime.Now.AddDays(1).Date;
            this.barEditTimeEnd.EditValue = zeroTime;
        }

        private void barButtonOk_ItemClick(object sender, ItemClickEventArgs e)
        {
            DateTime tmStart;
            DateTime tmEnd;
            try
            {
                tmStart = ((DateTime)this.barEditDataStart.EditValue).Date
                + ((DateTime)this.barEditTimeStart.EditValue).TimeOfDay;
                tmEnd = ((DateTime)this.barEditDateEnd.EditValue).Date
                    + ((DateTime)this.barEditTimeEnd.EditValue).TimeOfDay;
            }
            catch (System.Exception)
            {
                MessageBox.Show("请选择查询开始和结束的时间");
                return;
            }
            
            model.Query(tmStart, tmEnd);
            ReloadPreNextBtnState();
        }

        private void ViewEvents_Shown(object sender, EventArgs e)
        {
            NaviHelper.FormMain.ribbon.SelectedPage = NaviHelper.FormMain.ribbon.MergedPages["查询"];
        }

        

        

        

        

       

        
    }
}