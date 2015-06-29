using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QLite.Dialogs
{
    public partial class ServerLogin : Form
    {
        string conn = "";
        DialogResult result;

        public string Connection
        {
            get { return conn; }
        }

        public int AuthenticationType
        {
            get { return cboAuthentication.SelectedIndex; }
        }

        public DialogResult ClickResult
        {
            get { return result; }
        }

        public ServerLogin(ref string connection)
        {
            InitializeComponent();
            conn = connection;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            conn = txtMachine.Text + ";" + txtInstance.Text + ";" + txtUser.Text + ";" + txtPassword.Text;
            result = System.Windows.Forms.DialogResult.OK;
        }

        private void cboAuthentication_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cboAuthentication.SelectedIndex)
            {
                case 0: txtUser.Enabled = true; txtPassword.Enabled = true;
                    break;
                case 1: txtUser.Enabled = false; txtPassword.Enabled = false;
                    break;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            result = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void ServerLogin_Load(object sender, EventArgs e)
        {
            if (!conn.Equals(null))
            {
                if (Global.Defaults.Connection[1] == "")
                { txtMachine.Text = System.Environment.MachineName.ToString(); }
                else { txtMachine.Text = Global.Defaults.Connection[1]; }
                txtInstance.Text = Global.Defaults.Connection[0];
                txtUser.Text = Global.Defaults.Connection[2];
                txtPassword.Text = Global.Defaults.Connection[3];
            }
            else
            {
                string[] Conn = conn.Split(new char[] { ';' });
                txtMachine.Text = Conn[0]; txtInstance.Text = Conn[1];
                txtUser.Text = Conn[2]; txtPassword.Text = Conn[3];
            }
            txtPassword.UseSystemPasswordChar = QLite.Global.Defaults.PasswordLock;
        }
    }
}
