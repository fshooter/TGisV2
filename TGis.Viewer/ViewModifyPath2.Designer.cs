namespace TGis.Viewer
{
    partial class ViewModifyPath2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ribbon = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.barEditPathName = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.barBtnRedraw = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnSave = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnCancel = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnDelete = new DevExpress.XtraBars.BarButtonItem();
            this.barStaticInfo = new DevExpress.XtraBars.BarStaticItem();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonStatusBar = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            this.mapControl = new TGis.MapControl.MapControl();
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbon
            // 
            this.ribbon.ApplicationButtonText = null;
            // 
            // 
            // 
            this.ribbon.ExpandCollapseItem.Id = 0;
            this.ribbon.ExpandCollapseItem.Name = "";
            this.ribbon.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbon.ExpandCollapseItem,
            this.barEditPathName,
            this.barBtnRedraw,
            this.barBtnSave,
            this.barBtnCancel,
            this.barBtnDelete,
            this.barStaticInfo});
            this.ribbon.Location = new System.Drawing.Point(0, 0);
            this.ribbon.MaxItemId = 10;
            this.ribbon.Name = "ribbon";
            this.ribbon.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1});
            this.ribbon.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemTextEdit1});
            this.ribbon.SelectedPage = this.ribbonPage1;
            this.ribbon.Size = new System.Drawing.Size(442, 149);
            this.ribbon.StatusBar = this.ribbonStatusBar;
            // 
            // barEditPathName
            // 
            this.barEditPathName.Caption = "名称：";
            this.barEditPathName.Edit = this.repositoryItemTextEdit1;
            this.barEditPathName.Id = 1;
            this.barEditPathName.Name = "barEditPathName";
            this.barEditPathName.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barEditPathName.Width = 150;
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.MaxLength = 20;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // barBtnRedraw
            // 
            this.barBtnRedraw.Caption = "重绘";
            this.barBtnRedraw.Id = 2;
            this.barBtnRedraw.Name = "barBtnRedraw";
            this.barBtnRedraw.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barBtnRedraw.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnRedraw_ItemClick);
            // 
            // barBtnSave
            // 
            this.barBtnSave.Caption = "保存";
            this.barBtnSave.Enabled = false;
            this.barBtnSave.Id = 3;
            this.barBtnSave.Name = "barBtnSave";
            this.barBtnSave.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barBtnSave.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnSave_ItemClick);
            // 
            // barBtnCancel
            // 
            this.barBtnCancel.Caption = "取消";
            this.barBtnCancel.Id = 4;
            this.barBtnCancel.Name = "barBtnCancel";
            this.barBtnCancel.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barBtnCancel.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnCancel_ItemClick);
            // 
            // barBtnDelete
            // 
            this.barBtnDelete.Caption = "删除";
            this.barBtnDelete.Enabled = false;
            this.barBtnDelete.Id = 5;
            this.barBtnDelete.Name = "barBtnDelete";
            this.barBtnDelete.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barBtnDelete.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnDelete_ItemClick);
            // 
            // barStaticInfo
            // 
            this.barStaticInfo.Caption = "barStaticItem1";
            this.barStaticInfo.Id = 9;
            this.barStaticInfo.Name = "barStaticInfo";
            this.barStaticInfo.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1,
            this.ribbonPageGroup2});
            this.ribbonPage1.KeyTip = "编辑";
            this.ribbonPage1.Name = "ribbonPage1";
            this.ribbonPage1.Text = "编辑";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.barEditPathName);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            this.ribbonPageGroup1.Text = "路径名称";
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.AllowMinimize = false;
            this.ribbonPageGroup2.ItemLinks.Add(this.barBtnRedraw);
            this.ribbonPageGroup2.ItemLinks.Add(this.barBtnSave);
            this.ribbonPageGroup2.ItemLinks.Add(this.barBtnCancel);
            this.ribbonPageGroup2.ItemLinks.Add(this.barBtnDelete);
            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
            this.ribbonPageGroup2.Text = "功能";
            // 
            // ribbonStatusBar
            // 
            this.ribbonStatusBar.ItemLinks.Add(this.barStaticInfo);
            this.ribbonStatusBar.Location = new System.Drawing.Point(0, 418);
            this.ribbonStatusBar.Name = "ribbonStatusBar";
            this.ribbonStatusBar.Ribbon = this.ribbon;
            this.ribbonStatusBar.Size = new System.Drawing.Size(442, 31);
            // 
            // mapControl
            // 
            this.mapControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapControl.Location = new System.Drawing.Point(0, 149);
            this.mapControl.Name = "mapControl";
            this.mapControl.Size = new System.Drawing.Size(442, 269);
            this.mapControl.TabIndex = 2;
            // 
            // ViewModifyPath2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(442, 449);
            this.ControlBox = false;
            this.Controls.Add(this.mapControl);
            this.Controls.Add(this.ribbonStatusBar);
            this.Controls.Add(this.ribbon);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ViewModifyPath2";
            this.Ribbon = this.ribbon;
            this.StatusBar = this.ribbonStatusBar;
            this.Text = "修改路径信息";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ViewModifyPath_FormClosing);
            this.Load += new System.EventHandler(this.ViewModifyPath_Load);
            this.Shown += new System.EventHandler(this.ViewModifyPath2_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbon;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar;
        private DevExpress.XtraBars.BarEditItem barEditPathName;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
        private DevExpress.XtraBars.BarButtonItem barBtnRedraw;
        private DevExpress.XtraBars.BarButtonItem barBtnSave;
        private DevExpress.XtraBars.BarButtonItem barBtnCancel;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        private MapControl.MapControl mapControl;
        private DevExpress.XtraBars.BarButtonItem barBtnDelete;
        private DevExpress.XtraBars.BarStaticItem barStaticInfo;
    }
}