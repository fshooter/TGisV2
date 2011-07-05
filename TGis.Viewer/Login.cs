using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Security.Cryptography;

namespace TGis.Viewer
{
    public partial class Login : DevExpress.XtraEditors.XtraForm
    {
        public Login()
        {
            InitializeComponent();
        }

        
        private void Login_Load(object sender, EventArgs e)
        {
            string serverAddr;
            int serverPort;
            if (!GisGlobal.GetServerConnectionConf(out serverAddr, out serverPort))
                return;
            this.textEditServerIp.Text = serverAddr;
            this.textEditServerPort.Text = serverPort.ToString();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string serverAddr;
            int serverPort;
            try
            {
                serverAddr = textEditServerIp.Text;
                serverPort = Convert.ToInt32(textEditServerPort.Text);
            }
            catch (System.Exception)
            {
                MessageBox.Show("输入格式有误");
                return;
            }

            try
            {
                GisGlobal.SetServerConnectionConf(serverAddr, serverPort);
            }
            catch (System.Exception)
            {
                MessageBox.Show("写入配置文件失败");
                return;
            }
            try
            {
                string real_pass;
                string fake_pass = "abcdefghigklmnopqrstuvwxyz1234567890ABCDEFGHIGKLMNOPQRSTUVWXYZ";
                real_pass = fake_pass.Substring(26, 6);
                if (GisServiceWrapper.Instance.GetVersion() != 100)
                {
                    MessageBox.Show("客户端与服务器版本不匹配");
                    return;
                }
                SHA1 sha1 = SHA1.Create();

                byte[] pass = sha1.ComputeHash(Encoding.Default.GetBytes(real_pass));

                if (!GisServiceWrapper.Instance.VerifyPassword(pass))
                {
                    MessageBox.Show("密码错误");
                    return;
                }
            }
            catch (System.Exception)
            {
                MessageBox.Show("连接服务器失败,请检查您的网络连接");
                GisServiceWrapper.Instance.Close();
                GisServiceWrapper.Instance = null;
                return;
            }
            this.Visible = false;
            Program.MainStart();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}