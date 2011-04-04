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
            List_Load(sender, e);
            MapControl_Load(sender, e);
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
                row.Cells[1].Value = ei.Type;
                row.Cells[2].Value = car.Name;
            }
            dataGridView1.ResumeLayout(true);
        }
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count < 1)
                return;
            model.EventSelected = (GisEventInfo)dataGridView1.SelectedRows[0].Tag;
        }
#endregion

#region MapControl
        private int oldCarId = -1;
        private void MapControl_Load(object sender, EventArgs e)
        {
            mapControl.Navigate(Ultility.GetAppDir() + @"\map\map.html");
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
            oldCarId = ei.CarId;
        }
#endregion

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
        }

       

        
    }
}