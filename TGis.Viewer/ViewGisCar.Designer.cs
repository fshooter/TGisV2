namespace TGis.Viewer
{
    partial class ViewGisCar
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.mapControl1 = new TGis.MapControl.MapControl();
            this.table1 = new XPTable.Models.Table();
            this.columnModel = new XPTable.Models.ColumnModel();
            this.ColumnName = new XPTable.Models.TextColumn();
            this.ColumnX = new XPTable.Models.TextColumn();
            this.ColumnY = new XPTable.Models.TextColumn();
            this.ColumnRollDirection = new XPTable.Models.TextColumn();
            this.ColumnStatus = new XPTable.Models.TextColumn();
            this.tableModel = new XPTable.Models.TableModel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.table1)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(650, 408);
            this.splitContainer1.SplitterDistance = 149;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.mapControl1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.table1);
            this.splitContainer2.Size = new System.Drawing.Size(497, 408);
            this.splitContainer2.SplitterDistance = 320;
            this.splitContainer2.TabIndex = 0;
            // 
            // mapControl1
            // 
            this.mapControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapControl1.Location = new System.Drawing.Point(0, 0);
            this.mapControl1.Name = "mapControl1";
            this.mapControl1.Size = new System.Drawing.Size(497, 320);
            this.mapControl1.TabIndex = 0;
            // 
            // table1
            // 
            this.table1.ColumnModel = this.columnModel;
            this.table1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.table1.Location = new System.Drawing.Point(0, 0);
            this.table1.Name = "table1";
            this.table1.Size = new System.Drawing.Size(497, 84);
            this.table1.TabIndex = 0;
            this.table1.TableModel = this.tableModel;
            this.table1.Text = "table1";
            // 
            // columnModel
            // 
            this.columnModel.Columns.AddRange(new XPTable.Models.Column[] {
            this.ColumnName,
            this.ColumnX,
            this.ColumnY,
            this.ColumnRollDirection,
            this.ColumnStatus});
            // 
            // ColumnName
            // 
            this.ColumnName.Text = "名称";
            // 
            // ColumnX
            // 
            this.ColumnX.Text = "经度";
            // 
            // ColumnY
            // 
            this.ColumnY.Text = "纬度";
            // 
            // ColumnRollDirection
            // 
            this.ColumnRollDirection.Text = "转动方向";
            // 
            // ColumnStatus
            // 
            this.ColumnStatus.Text = "连线状态";
            // 
            // ViewGisCar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(650, 408);
            this.Controls.Add(this.splitContainer1);
            this.Name = "ViewGisCar";
            this.Text = "ViewGisCar";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ViewGisCar_FormClosing);
            this.Load += new System.EventHandler(this.ViewGisCar_Load);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.table1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private MapControl.MapControl mapControl1;
        private XPTable.Models.Table table1;
        private XPTable.Models.ColumnModel columnModel;
        private XPTable.Models.TableModel tableModel;
        private XPTable.Models.TextColumn ColumnName;
        private XPTable.Models.TextColumn ColumnX;
        private XPTable.Models.TextColumn ColumnY;
        private XPTable.Models.TextColumn ColumnRollDirection;
        private XPTable.Models.TextColumn ColumnStatus;

    }
}