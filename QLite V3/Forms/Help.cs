using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using QLite.Engines;

namespace QLite.Forms
{
    public partial class Help : Form
    {
        #region Globals
        private QMain _Engine = new QMain();
        private static QXML _XmlEngine = new QXML();
        private XmlDocument qlite = new XmlDocument();
        private string[,] helpTopics = new string[QXML.CountTwoDeep("Help", "Topic"), 2];
        private string[,] helpContent = new string[QXML.CountTwoDeep("Help", "Content"), 3];

        #endregion

        public Help()
            : base()
        {
            qlite.Load("QLite2.xml");
            InitializeComponent();
            this.Icon = new System.Drawing.Icon(QMain.GetResImage("q2icon.ico"));
        }

        private void Form_Load(object sender, EventArgs e)
        {
            helpTopics = _XmlEngine.PullHelpTopics();
            helpContent = _XmlEngine.PullHelpContent();

            for (int x = 0; x <= helpTopics.GetUpperBound(0); x++)
            {
                TreeNode tmpNode = new TreeNode(helpTopics[x, 1]);
                tmpNode.Tag = helpTopics[x, 0].ToString();
                treTopics.Nodes.Add(tmpNode);
            }
        }

        private void treTopics_AfterSelect(object sender, EventArgs e)
        {
            TreeView tmpView = (TreeView)sender;
            TreeNode tmpSelectedNode = tmpView.SelectedNode;

            tmpSelectedNode.Nodes.Clear();

            if (tmpSelectedNode.Parent == null)
            {
                for (int x = 0; x <= helpContent.GetUpperBound(0); x++)
                    if (tmpSelectedNode.Tag.ToString() == helpContent[x, 1])
                    {
                        TreeNode tmpNode = new TreeNode(helpContent[x, 0]);
                        tmpNode.Tag = helpContent[x, 1];
                        tmpSelectedNode.Nodes.Add(tmpNode);
                    }
            }
            else
                for (int x = 0; x <= helpContent.GetUpperBound(0); x++)
                    if (tmpSelectedNode.Tag.ToString() == helpContent[x, 1] && tmpSelectedNode.Text == helpContent[x, 0])
                        txtInfo.Text = helpContent[x, 2];
        }

    }
}
