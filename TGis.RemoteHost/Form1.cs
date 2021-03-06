﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel;
using TGis.RemoteService;

namespace TGis.RemoteHost
{
    public partial class Form1 : Form
    {
        private ServiceHost host;
        
        public Form1()
        {
            InitializeComponent();
            host = new ServiceHost(typeof(TGis.RemoteService.ServiceImpl));
            host.Open();
            TGis.RemoteService.UdpCarTerminalAbility.tcp_server.OnMsg += new MsgShowHandler(MsgShower);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (host != null)
            {
                MessageBox.Show("服务已经启动");
                return;
            }
            
            host = new ServiceHost(typeof(TGis.RemoteService.ServiceImpl));
            host.Open();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (host != null)
            {
                host.Close();
                host = null;
            }
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (host != null)
            {
                host.Close();
                host = null;
            }
            this.Close();
        }

        private bool windowCreate=true;
        protected override void OnActivated(EventArgs e) 
        { 
            if (windowCreate) 
            { 
                base.Visible = false;
                windowCreate = false;
            } 

            base.OnActivated(e); 
        }
        private void MsgShower(string msg)
        {
            this.BeginInvoke(new MsgShowHandler(SynMsgShower), new object[] { msg });
        }
        private void SynMsgShower(string msg)
        {
            notifyIcon1.ShowBalloonTip(5000, "", msg, ToolTipIcon.Info);
        }
    }
}
