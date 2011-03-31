namespace TGis.Viewer
{
    partial class ViewMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.模式ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.欢迎界面ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.即时模式ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.历史模式ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.模式ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(284, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 模式ToolStripMenuItem
            // 
            this.模式ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.欢迎界面ToolStripMenuItem,
            this.即时模式ToolStripMenuItem,
            this.历史模式ToolStripMenuItem});
            this.模式ToolStripMenuItem.Name = "模式ToolStripMenuItem";
            this.模式ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.模式ToolStripMenuItem.Text = "模式";
            // 
            // 欢迎界面ToolStripMenuItem
            // 
            this.欢迎界面ToolStripMenuItem.Name = "欢迎界面ToolStripMenuItem";
            this.欢迎界面ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.欢迎界面ToolStripMenuItem.Text = "欢迎界面";
            // 
            // 即时模式ToolStripMenuItem
            // 
            this.即时模式ToolStripMenuItem.Name = "即时模式ToolStripMenuItem";
            this.即时模式ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.即时模式ToolStripMenuItem.Text = "即时模式";
            this.即时模式ToolStripMenuItem.Click += new System.EventHandler(this.即时模式ToolStripMenuItem_Click);
            // 
            // 历史模式ToolStripMenuItem
            // 
            this.历史模式ToolStripMenuItem.Name = "历史模式ToolStripMenuItem";
            this.历史模式ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.历史模式ToolStripMenuItem.Text = "历史模式";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Location = new System.Drawing.Point(0, 25);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(284, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // ViewMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ViewMain";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 模式ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 欢迎界面ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 即时模式ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 历史模式ToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
    }
}

