using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TGis.Common;
using TGis.MapControl;

namespace TGis.Viewer
{
    partial class ViewModifyPath : Form
    {
        private ModifyPathModel model;
        private bool bDrawingMode = false;
        private bool bInitComplecate = false;
        public ViewModifyPath(ModifyPathModel model)
        {
            this.model = model;
            InitializeComponent();
        }

        private void ViewModifyPath_Load(object sender, EventArgs e)
        {
            mapControl.Navigate(Ultility.GetAppDir() + @"\map\map.html");
            mapControl.OnMapLoadCompleted += new MapLoadCompleteHandler(InitMapFirstTime);
            //GisGlobal.GPathMgr.OnPathStateChanged += new PathStateChangeHandler(PathState_Change);
        }
        private void InitMapFirstTime(object sender)
        {
            bInitComplecate = true;
            PathState_Change(this, null);
        }
        private void ViewModifyPath_FormClosing(object sender, FormClosingEventArgs e)
        {
           // GisGlobal.GPathMgr.OnPathStateChanged -= new PathStateChangeHandler(PathState_Change);
        }

        private void ReloadPath(object sender, EventArgs arg)
        {
            if (bDrawingMode || !bInitComplecate) return;
            try
            {
                mapControl.RemovePath(model.Id);
            }
            catch (System.Exception)
            {
            	
            }
            Path path;
            if (!GisGlobal.GPathMgr.TryGetPath(model.Id, out path))
                return;
            mapControl.AddPath(path.Id, path.Name, path.PathPolygon.Points);
        }
        private void PathState_Change(object sender, PathStateChangeArgs arg)
        {
            this.BeginInvoke(new EventHandler(ReloadPath), new object[] { this, null });
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            double[] points = mapControl.EndDrawPath();
            if (bDrawingMode && (points.Length < 6))
            {
                MessageBox.Show("尚未绘制路径");
                return;
            }
            Path path;
            if (!GisGlobal.GPathMgr.TryGetPath(model.Id, out path))
                return;
            Path newp = new Path(path.Id, path.Name, points);
            GisGlobal.GPathMgr.UpdatePath(newp);
            NaviHelper.NaviToWelcome();
        }

        private void btnRedraw_Click(object sender, EventArgs e)
        {
            if (bDrawingMode)
                mapControl.EndDrawPath();
            bDrawingMode = true;
            try
            {
                mapControl.RemovePath(model.Id);
            }
            catch (System.Exception)
            {
            	
            }
           
            mapControl.BeginDrawPath();
        }
    }
}
