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
    partial class ViewModifyPath2 : DevExpress.XtraBars.Ribbon.RibbonForm
    {
       private ModifyPathModel model;
        private bool bDrawingMode = false;
        private bool bInitComplecate = false;
        public ViewModifyPath2(ModifyPathModel model)
        {
            this.model = model;
            InitializeComponent();
        }

        private void ViewModifyPath_Load(object sender, EventArgs e)
        {
            this.ribbon.SelectedPage = ribbonPage1;
            mapControl.Navigate(Ultility.GetAppDir() + @"\map\map.html");
            mapControl.OnMapLoadCompleted += new MapLoadCompleteHandler(InitMapFirstTime);
        }
        private void InitMapFirstTime(object sender)
        {
            bInitComplecate = true;
            PathState_Change(this, null);
        }
        private void ViewModifyPath_FormClosing(object sender, FormClosingEventArgs e)
        {
           
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
            GisPathInfo path;
            if (!GisGlobal.GPathMgr.TryGetPath(model.Id, out path))
                return;
            barEditPathName.EditValue = path.Name;
            mapControl.AddPath(path.Id, path.Name, path.Points);
        }
        private void PathState_Change(object sender, EventArgs arg)
        {
            this.BeginInvoke(new EventHandler(ReloadPath), new object[] { this, null });
        }

        private void barBtnRedraw_ItemClick(object sender, ItemClickEventArgs e)
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

        private void barBtnSave_ItemClick(object sender, ItemClickEventArgs e)
        {
            GisPathInfo path;
            if (!GisGlobal.GPathMgr.TryGetPath(model.Id, out path))
                return;
            double[] points;
            if (bDrawingMode)
            {
                points = mapControl.EndDrawPath();
                if ((points == null) || (points.Length < 6))
                {
                    MessageBox.Show("尚未绘制路径");
                    return;
                }
            }
            else
                points = path.Points;
            
            string newName = (string)barEditPathName.EditValue;
            foreach (GisPathInfo p in GisGlobal.GPathMgr.Paths)
            {
                if((p.Id != model.Id) && (p.Name == newName))
                {
                    MessageBox.Show("有重名的路径，请更改路径名称");
                    return;
                }
            }
            GisPathInfo newp = new GisPathInfo();
            newp.Id = model.Id;
            newp.Name = newName;
            newp.Points = points;
            GisGlobal.GPathMgr.UpdatePath(newp);
            NaviHelper.NaviToWelcome();
        }

        private void barBtnCancel_ItemClick(object sender, ItemClickEventArgs e)
        {
            NaviHelper.NaviToWelcome();
        }

        private void barBtnDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            GisPathInfo path;
            if (!GisGlobal.GPathMgr.TryGetPath(model.Id, out path))
                return;
            GisGlobal.GPathMgr.RemovePath(path.Id);
            NaviHelper.NaviToWelcome();
        }
    }
}