namespace TGis.Viewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewMain2));
            this.ribbon = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.barBtnWelcomeMode = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnImmMode = new DevExpress.XtraBars.BarButtonItem();
            this.barbtnHistoryMode = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonNewCar = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnNewPath = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonEventsMode = new DevExpress.XtraBars.BarButtonItem();
            this.editNewMap = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.editNewPass = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.btnModifyPass = new DevExpress.XtraBars.BarButtonItem();
            this.btnChangeMap = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPageMode = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageCar = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroupAllCars = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPagePath = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup3 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroupAllPaths = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup4 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup5 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbon
            // 
            this.ribbon.ApplicationButtonText = null;
            this.ribbon.ApplicationIcon = global::TGis.Viewer.Properties.Resources.client;
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
            this.barBtnNewPath,
            this.barButtonEventsMode,
            this.editNewMap,
            this.editNewPass,
            this.btnModifyPass,
            this.btnChangeMap});
            this.ribbon.Location = new System.Drawing.Point(0, 0);
            this.ribbon.MaxItemId = 17;
            this.ribbon.Name = "ribbon";
            this.ribbon.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPageMode,
            this.ribbonPageCar,
            this.ribbonPagePath,
            this.ribbonPage1});
            this.ribbon.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemComboBox1,
            this.repositoryItemTextEdit1});
            this.ribbon.SelectedPage = this.ribbonPageMode;
            this.ribbon.Size = new System.Drawing.Size(442, 149);
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
            this.barbtnHistoryMode.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barbtnHistoryMode_ItemClick);
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
            // barButtonEventsMode
            // 
            this.barButtonEventsMode.Caption = "事件查询";
            this.barButtonEventsMode.Id = 11;
            this.barButtonEventsMode.Name = "barButtonEventsMode";
            this.barButtonEventsMode.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonEventsMode.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonEventsMode_ItemClick);
            // 
            // editNewMap
            // 
            this.editNewMap.Caption = "选择地图：";
            this.editNewMap.Edit = this.repositoryItemComboBox1;
            this.editNewMap.Id = 12;
            this.editNewMap.Name = "editNewMap";
            this.editNewMap.Width = 100;
            // 
            // repositoryItemComboBox1
            // 
            this.repositoryItemComboBox1.AutoHeight = false;
            this.repositoryItemComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
            this.repositoryItemComboBox1.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            // 
            // editNewPass
            // 
            this.editNewPass.Caption = "修改密码";
            this.editNewPass.Edit = this.repositoryItemTextEdit1;
            this.editNewPass.Id = 13;
            this.editNewPass.Name = "editNewPass";
            this.editNewPass.Width = 100;
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // btnModifyPass
            // 
            this.btnModifyPass.Caption = "确定";
            this.btnModifyPass.Id = 14;
            this.btnModifyPass.Name = "btnModifyPass";
            this.btnModifyPass.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.btnModifyPass.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnModifyPass_ItemClick);
            // 
            // btnChangeMap
            // 
            this.btnChangeMap.Caption = "确定";
            this.btnChangeMap.Id = 15;
            this.btnChangeMap.Name = "btnChangeMap";
            this.btnChangeMap.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.btnChangeMap.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnChangeMap_ItemClick);
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
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonEventsMode);
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
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup4,
            this.ribbonPageGroup5});
            this.ribbonPage1.Name = "ribbonPage1";
            this.ribbonPage1.Text = "设置";
            // 
            // ribbonPageGroup4
            // 
            this.ribbonPageGroup4.ItemLinks.Add(this.editNewPass);
            this.ribbonPageGroup4.ItemLinks.Add(this.btnModifyPass);
            this.ribbonPageGroup4.Name = "ribbonPageGroup4";
            this.ribbonPageGroup4.Text = "密码设置";
            // 
            // ribbonPageGroup5
            // 
            this.ribbonPageGroup5.ItemLinks.Add(this.editNewMap);
            this.ribbonPageGroup5.ItemLinks.Add(this.btnChangeMap);
            this.ribbonPageGroup5.Name = "ribbonPageGroup5";
            this.ribbonPageGroup5.Text = "地图设置";
            // 
            // ViewMain2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(442, 445);
            this.Controls.Add(this.ribbon);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Name = "ViewMain2";
            this.Ribbon = this.ribbon;
            this.Text = "Gps客户端";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ViewMain_FormClosing);
            this.Load += new System.EventHandler(this.ViewMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPageMode;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
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
        private DevExpress.XtraBars.BarButtonItem barButtonEventsMode;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup4;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup5;
        private DevExpress.XtraBars.BarEditItem editNewMap;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox1;
        private DevExpress.XtraBars.BarEditItem editNewPass;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
        private DevExpress.XtraBars.BarButtonItem btnModifyPass;
        private DevExpress.XtraBars.BarButtonItem btnChangeMap;
        public DevExpress.XtraBars.Ribbon.RibbonControl ribbon;
    }
}