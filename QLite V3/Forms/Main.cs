using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using QLite.Controls;
using QLite.Engines;
using QLite.Servers;
using QLite.Plugins;

namespace QLite.Forms
{
    public partial class frmMain : Form
    {


        public frmMain()
        { InitializeComponent(); }

        private void frmMain_Load(object sender, EventArgs e)
        {
            if (QMain.LoadPlugins() != -1)
            {
                foreach (var p in Global.Plugins.Catalog.Parts)
                    foreach (var ed in p.ExportDefinitions)
                        if (ed.ContractName == "IPlugin")
                            foreach (var meta in ed.Metadata)
                                if (meta.Key == "Name")
                                {
                                    ToolStripMenuItem item = new ToolStripMenuItem(meta.Value.ToString());
                                    item.Tag = meta.Value;
                                    item.Click += new EventHandler(Utility_Click);
                                    mnuUtilities.DropDownItems.Add(item);
                                }
            }
        }

        private void miNewQuery_Click(object sender, EventArgs e)
        {
            int procNum = QMain.StartProcess(sender as ToolStripItem, new Server());
            if (procNum != -1)
            {
                UserControl tmpUI = Global.Processes[procNum].UI;
                QControl.SetProcessMenu(tmpUI, ref tspMainStrip);
                qtabsMain.Add(tmpUI, procNum);
            }
        }

        private void Utility_Click(object sender, EventArgs e)
        {
            int procNum = QMain.StartProcess(sender as ToolStripMenuItem, new Server());
            if (procNum != -1)
                foreach(var plug in Global.Plugins.Catalog.Parts)
                    foreach(var ed in plug.ExportDefinitions)
                        foreach (var meta in ed.Metadata) { }
                            /*if (meta.Value == "Name" && meta.Value == sender as string)
                            {
                                
                            }*/
                //qtabsMain.Add(Global.Plugins.AvailablePlugins[Global.Processes[procNum].PluginIndex].Instance.UI, procNum);
                //tspMainStrip.Join(Global.Plugins.AvailablePlugins[Global.Processes[procNum].PluginIndex].Instance.Menu);

        }

        private void qtabsMain_Selected(object sender, TabControlEventArgs e)
        {
            TabPage tmpNew = (sender as TabControl).SelectedTab;
            int procNum = Convert.ToInt32(tmpNew.Tag);
            tspMainStrip.Controls.Clear();
            //tspMainStrip.Join(Global.Plugins.AvailablePlugins[Global.Processes[procNum].PluginIndex].Instance.Menu);
        }

        private void miEditOptions_Click(object sender, EventArgs e)
        {
            Options dlgOptions = new Options();
            dlgOptions.Show();
        }

    }
}
