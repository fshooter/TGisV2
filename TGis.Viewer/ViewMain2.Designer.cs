﻿namespace TGis.Viewer
{
    partial class ViewMain2
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
            this.barBtnWelcomeMode = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnImmMode = new DevExpress.XtraBars.BarButtonItem();
            this.barbtnHistoryMode = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonNewCar = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnNewPath = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPageMode = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageCar = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroupAllCars = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPagePath = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup3 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroupAllPaths = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonStatusBar = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).BeginInit();
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
            this.barBtnWelcomeMode,
            this.barBtnImmMode,
            this.barbtnHistoryMode,
            this.barButtonNewCar,
            this.barBtnNewPath});
            this.ribbon.Location = new System.Drawing.Point(0, 0);
            this.ribbon.MaxItemId = 11;
            this.ribbon.Name = "ribbon";
            this.ribbon.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPageMode,
            this.ribbonPageCar,
            this.ribbonPagePath});
            this.ribbon.SelectedPage = this.ribbonPagePath;
            this.ribbon.Size = new System.Drawing.Size(442, 149);
            this.ribbon.StatusBar = this.ribbonStatusBar;
            // 
            // barBtnWelcomeMode
            // 
            this.barBtnWelcomeMode.Caption = "欢迎界面";
            this.barBtnWelcomeMode.Id = 5;
            this.barBtnWelcomeMode.LargeWidth = 100;
            this.barBtnWelcomeMode.Name = "barBtnWelcomeMode";
            this.barBtnWelcomeMode.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barBtnWelcomeMode.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnWelcomeMode_Click);
            // 
            // barBtnImmMode
            // 
            this.barBtnImmMode.Caption = "即时模式";
            this.barBtnImmMode.Id = 6;
            this.barBtnImmMode.LargeWidth = 100;
            this.barBtnImmMode.Name = "barBtnImmMode";
            this.barBtnImmMode.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barBtnImmMode.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnImmMode_ItemClick);
            // 
            // barbtnHistoryMode
            // 
            this.barbtnHistoryMode.Caption = "历史模式";
            this.barbtnHistoryMode.Id = 7;
            this.barbtnHistoryMode.LargeWidth = 100;
            this.barbtnHistoryMode.Name = "barbtnHistoryMode";
            this.barbtnHistoryMode.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            // 
            // barButtonNewCar
            // 
            this.barButtonNewCar.Caption = "新建车辆";
            this.barButtonNewCar.Id = 9;
            this.barButtonNewCar.Name = "barButtonNewCar";
            this.barButtonNewCar.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonNewCar.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonNewCar_ItemClick);
            // 
            // barBtnNewPath
            // 
            this.barBtnNewPath.Caption = "新建路径";
            this.barBtnNewPath.Id = 10;
            this.barBtnNewPath.Name = "barBtnNewPath";
            this.barBtnNewPath.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barBtnNewPath.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnNewPath_ItemClick);
            // 
            // ribbonPageMode
            // 
            this.ribbonPageMode.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1});
            this.ribbonPageMode.Name = "ribbonPageMode";
            this.ribbonPageMode.Text = "查看";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.barBtnWelcomeMode);
            this.ribbonPageGroup1.ItemLinks.Add(this.barBtnImmMode);
            this.ribbonPageGroup1.ItemLinks.Add(this.barbtnHistoryMode);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            this.ribbonPageGroup1.Text = "显示模式";
            // 
            // ribbonPageCar
            // 
            this.ribbonPageCar.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup2,
            this.ribbonPageGroupAllCars});
            this.ribbonPageCar.Name = "ribbonPageCar";
            this.ribbonPageCar.Text = "车辆";
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.ItemLinks.Add(this.barButtonNewCar);
            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
            this.ribbonPageGroup2.Text = "新建";
            // 
            // ribbonPageGroupAllCars
            // 
            this.ribbonPageGroupAllCars.AllowMinimize = false;
            this.ribbonPageGroupAllCars.Name = "ribbonPageGroupAllCars";
            this.ribbonPageGroupAllCars.Text = "所有车辆";
            // 
            // ribbonPagePath
            // 
            this.ribbonPagePath.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup3,
            this.ribbonPageGroupAllPaths});
            this.ribbonPagePath.Name = "ribbonPagePath";
            this.ribbonPagePath.Text = "路径";
            // 
            // ribbonPageGroup3
            // 
            this.ribbonPageGroup3.ItemLinks.Add(this.barBtnNewPath);
            this.ribbonPageGroup3.Name = "ribbonPageGroup3";
            this.ribbonPageGroup3.Text = "新建";
            // 
            // ribbonPageGroupAllPaths
            // 
            this.ribbonPageGroupAllPaths.AllowMinimize = false;
            this.ribbonPageGroupAllPaths.Name = "ribbonPageGroupAllPaths";
            this.ribbonPageGroupAllPaths.Text = "所有路径";
            // 
            // ribbonStatusBar
            // 
            this.ribbonStatusBar.Location = new System.Drawing.Point(0, 418);
            this.ribbonStatusBar.Name = "ribbonStatusBar";
            this.ribbonStatusBar.Ribbon = this.ribbon;
            this.ribbonStatusBar.Size = new System.Drawing.Size(442, 31);
            // 
            // ViewMain2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(442, 449);
            this.Controls.Add(this.ribbonStatusBar);
            this.Controls.Add(this.ribbon);
            this.IsMdiContainer = true;
            this.Name = "ViewMain2";
            this.Ribbon = this.ribbon;
            this.StatusBar = this.ribbonStatusBar;
            this.Text = "ViewMain2";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ViewMain_FormClosing);
            this.Load += new System.EventHandler(this.ViewMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbon;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPageMode;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPageCar;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPagePath;
        private DevExpress.XtraBars.BarButtonItem barBtnWelcomeMode;
        private DevExpress.XtraBars.BarButtonItem barBtnImmMode;
        private DevExpress.XtraBars.BarButtonItem barbtnHistoryMode;
        private DevExpress.XtraBars.BarButtonItem barButtonNewCar;
        private DevExpress.XtraBars.BarButtonItem barBtnNewPath;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroupAllCars;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup3;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroupAllPaths;
    }
}