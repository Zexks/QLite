using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using QLite.Objects;

namespace QLite.Engines
{
    public class QDialog : IDisposable
    {
        public static DialogResult ServerLogonDialog(ref string value)
        {
            Form form = new Form();
            TableLayoutPanel tlpController = new TableLayoutPanel();
            FlowLayoutPanel flwPanel = new FlowLayoutPanel();
            Label lblMachine = new Label();
            Label lblInstance = new Label();
            Label lblLogin = new Label();
            Label lblPassword = new Label();
            TextBox txtMachine = new TextBox();
            TextBox txtInstance = new TextBox();
            TextBox txtLogin = new TextBox();
            TextBox txtPassword = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            #region Dialog Control Init
            lblMachine.Text = "Machine Name:";
            lblInstance.Text = "Instance Name:";
            lblLogin.Text = "Login:";
            lblPassword.Text = "Password:";

            lblMachine.TextAlign = ContentAlignment.MiddleRight;
            lblInstance.TextAlign = ContentAlignment.MiddleRight;
            lblLogin.TextAlign = ContentAlignment.MiddleRight;
            lblPassword.TextAlign = ContentAlignment.MiddleRight;

            lblMachine.Dock = DockStyle.Fill;
            lblInstance.Dock = DockStyle.Fill;
            lblLogin.Dock = DockStyle.Fill;
            lblPassword.Dock = DockStyle.Fill;

            txtMachine.Name = "txtMachine";
            txtInstance.Name = "txtInstance";
            txtLogin.Name = "txtLogin";
            txtPassword.Name = "txtPassword";

            if (value.Equals(""))
            {
                if (Global.Defaults.Connection[1] == "")
                { txtMachine.Text = System.Environment.MachineName.ToString(); }
                else { txtMachine.Text = Global.Defaults.Connection[1]; }
                txtInstance.Text = Global.Defaults.Connection[0];
                txtLogin.Text = Global.Defaults.Connection[2];
                txtPassword.Text = Global.Defaults.Connection[3];
            }
            else
            {
                string[] conn = value.Split(new char[] { ';' });
                txtMachine.Text = conn[0]; txtInstance.Text = conn[1];
                txtLogin.Text = conn[2]; txtPassword.Text = conn[3];
            }

            txtMachine.Dock = DockStyle.Fill;
            txtInstance.Dock = DockStyle.Fill;
            txtLogin.Dock = DockStyle.Fill;
            txtPassword.Dock = DockStyle.Fill;

            txtPassword.UseSystemPasswordChar = Global.Defaults.PasswordLock;

            //tlpController
            tlpController.Name = "tlpController";
            tlpController.ColumnCount = 2;
            tlpController.RowCount = 5;
            tlpController.Dock = DockStyle.Fill;
            //tlpController.BorderStyle = BorderStyle.Fixed3D;
            //tlpController.CellBorderStyle = TableLayoutPanelCellBorderStyle.Outset;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            tlpController.Controls.Add(lblMachine, 0, 0);
            tlpController.Controls.Add(lblInstance, 0, 1);
            tlpController.Controls.Add(lblLogin, 0, 2);
            tlpController.Controls.Add(lblPassword, 0, 3);
            tlpController.Controls.Add(txtMachine, 1, 0);
            tlpController.Controls.Add(txtInstance, 1, 1);
            tlpController.Controls.Add(txtLogin, 1, 2);
            tlpController.Controls.Add(txtPassword, 1, 3);
            tlpController.Controls.Add(flwPanel, 1, 4);

            flwPanel.Controls.Add(buttonOk);
            flwPanel.Controls.Add(buttonCancel);

            #endregion

            form.Text = "Connect to Server";
            form.ClientSize = new Size(400, 175);
            form.Controls.AddRange(new Control[] { tlpController });
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;
            DialogResult dialogResult = form.ShowDialog();
            value = txtMachine.Text + ";" + txtInstance.Text + ";" + txtLogin.Text + ";" + txtPassword.Text + ";10";
            return dialogResult;
        }

        public static DialogResult InputBox(string title, string promptText, ref string value)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 10, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;
            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }

        public static DialogResult UnsavedQueries()
        {
            Form dialog = new Form();
            TableLayoutPanel tlpMainLayout = new TableLayoutPanel();
            FlowLayoutPanel flpButtons = new FlowLayoutPanel();
            Label lblInfo = new Label();
            Button btnSave = new Button();
            Button btnDSave = new Button();

            tlpMainLayout.Name = "tlpMainLayout";
            tlpMainLayout.Dock = DockStyle.Fill;
            tlpMainLayout.ColumnCount = 1;
            tlpMainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            tlpMainLayout.RowCount = 2;
            tlpMainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 70));
            tlpMainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 30));

            flpButtons.Name = "flpButtons";
            flpButtons.Dock = DockStyle.Fill;
            flpButtons.FlowDirection = FlowDirection.RightToLeft;
            flpButtons.Controls.AddRange(new Control[] { btnDSave, btnSave });

            dialog.Text = "Unsaved Changes";
            lblInfo.Text = "You currently have unsaved changes to Queries stored in memory.\n\nWould you like to continue?";
            lblInfo.AutoSize = true;

            btnSave.Text = "&Save";
            btnSave.DialogResult = DialogResult.Yes;
            btnDSave.Text = "&Don't Save";

            tlpMainLayout.Controls.Add(lblInfo, 0, 0);
            tlpMainLayout.Controls.Add(flpButtons, 0, 1);

            dialog.ClientSize = new Size(396, 107);
            dialog.Controls.Add(tlpMainLayout);
            dialog.ClientSize = new Size(Math.Max(300, lblInfo.Right + 10), dialog.ClientSize.Height);
            dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
            dialog.StartPosition = FormStartPosition.CenterScreen;
            dialog.MinimizeBox = false;
            dialog.MaximizeBox = false;
            dialog.AcceptButton = btnSave;
            dialog.CancelButton = btnDSave;
            DialogResult dialogResult = dialog.ShowDialog();
            return dialogResult;
        }

        public static void ShowPrintDialog(object sender, WebBrowserNavigatedEventArgs e)
        {
            WebBrowser wb = sender as WebBrowser;
            PrintDialogSettings settings = wb.Tag as PrintDialogSettings;
            //prepare export with wordwrapping
            ExportToHTML exporter = new ExportToHTML();
            exporter.UseBr = true;
            exporter.UseForwardNbsp = true;
            exporter.UseNbsp = false;
            exporter.UseStyleTag = false;
            //generate HTML
            string HTML = exporter.GetHtml(settings.printRange);
            //show print dialog
            wb.Document.Body.InnerHtml = HTML;
            if (settings.ShowPrintPreviewDialog)
                wb.ShowPrintPreviewDialog();
            else
            {
                if (settings.ShowPageSetupDialog)
                    wb.ShowPageSetupDialog();

                if (settings.ShowPrintDialog)
                    wb.ShowPrintDialog();
                else
                    wb.Print();
            }
            //destroy webbrowser
            wb.Parent = null;
            wb.Dispose();
        }

        public static void ScrollableMsgBox(string promptText, string title)
        {
            Form form = new Form();
            RichTextBox txtMessage = new RichTextBox();
            Button buttonOk = new Button();

            txtMessage.Text = promptText;
            txtMessage.Enabled = true;
            txtMessage.Multiline = true;
            txtMessage.ReadOnly = true;

            buttonOk.Text = "OK";
            buttonOk.DialogResult = DialogResult.OK;

            txtMessage.SetBounds(5, 5, 450, 215);
            buttonOk.SetBounds(190, 225, 75, 23);

            txtMessage.AutoSize = true;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.Text = title;
            form.ClientSize = new Size(450, 250);
            form.Controls.AddRange(new Control[] { txtMessage, buttonOk });
            form.ClientSize = new Size(Math.Max(300, txtMessage.Right + 5), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.AcceptButton = buttonOk;
            form.ShowDialog();
        }

        public static void AboutDialog()
        {
            XmlNode tmpNode = QXML.PullTwoDeep("Help", "About");
            XmlAttributeCollection attAbout = tmpNode.Attributes;

            Form form = new Form();
            TableLayoutPanel tlpMainLayout = new TableLayoutPanel();
            Panel pnlBackGround = new Panel();
            PictureBox pbLogo = new PictureBox();
            Label lblTitle = new Label();
            Label lblVersion = new Label();
            Label lblAuthor = new Label();
            Button btnOK = new Button();

            //pnlBackGround
            pnlBackGround.Name = "pnlBackGround";
            pnlBackGround.Dock = DockStyle.Fill;
            pnlBackGround.Controls.Add(pbLogo);

            //pbLogo
            pbLogo.Name = "pbLogo";
            pbLogo.Dock = DockStyle.Fill;
            pbLogo.SizeMode = PictureBoxSizeMode.Zoom;
            pbLogo.Image = Image.FromStream(QMain.GetResImage("q2icon.ico"));

            //lblTitle
            lblTitle.Name = "lblTitle";
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.Font = new Font(lblTitle.Font.Name, 16);
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            lblTitle.Text = "Q-Lite";

            //lblVersion
            lblVersion.Name = "lblVersion";
            lblVersion.Dock = DockStyle.Fill;
            lblVersion.TextAlign = ContentAlignment.MiddleCenter;
            lblVersion.Text = "Version:\n\r" + attAbout[1].Value.ToString();

            //lblAuthor
            lblAuthor.Name = "lblAuthor";
            lblAuthor.Dock = DockStyle.Fill;
            lblAuthor.TextAlign = ContentAlignment.MiddleCenter;
            lblAuthor.Text = "Author:\n\r" + attAbout[0].Value.ToString();

            //btnOK
            btnOK.Name = "btnOK";
            btnOK.Text = "OK";
            btnOK.Dock = DockStyle.Fill;
            btnOK.DialogResult = DialogResult.OK;

            //tlpMainLayout
            tlpMainLayout.Name = "tlpMainLayout";
            tlpMainLayout.ColumnCount = 1;
            tlpMainLayout.RowCount = 5;
            tlpMainLayout.Dock = DockStyle.Fill;
            //tlpMainLayout.BorderStyle = BorderStyle.Fixed3D;
            //tlpMainLayout.CellBorderStyle = TableLayoutPanelCellBorderStyle.Outset;

            tlpMainLayout.Controls.Add(lblTitle, 0, 0);
            tlpMainLayout.Controls.Add(pnlBackGround, 0, 1);
            tlpMainLayout.Controls.Add(lblVersion, 0, 2);
            tlpMainLayout.Controls.Add(lblAuthor, 0, 3);
            tlpMainLayout.Controls.Add(btnOK, 0, 4);

            tlpMainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 10));
            tlpMainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 65));
            tlpMainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 10));
            tlpMainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 15));

            form.Text = "About";
            form.ClientSize = new Size(225, 350);
            form.Controls.Add(tlpMainLayout);
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.AcceptButton = btnOK;
            form.ShowDialog();

        }

        #region Resource Cleanup
        private bool IsDisposed = false;

        public void Free()
        {
            if (IsDisposed)
            { throw new System.ObjectDisposedException("Object Name"); }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~QDialog()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposedStatus)
        {
            if (!IsDisposed)
            {
                IsDisposed = true;
                // Released unmanaged Resources
                if (disposedStatus)
                {
                    // Released managed Resources
                }
            }
        }

        #endregion
    }

}
