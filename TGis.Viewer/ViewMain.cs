using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TGis.Viewer
{
    partial class ViewMain : Form
    {
        private MainToolController controller;
        private MainToolModel model;
        public ViewMain(MainToolController controller, MainToolModel model)
        {
            this.controller = controller;
            this.model = model;
            InitializeComponent();
        }

        private void 即时模式ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            controller.ImmediateMode();
        }
    }
}
