using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using QLite.Structures;

namespace QLite.Engines
{
    public class QControl
    {
        public static void SetProcessMenu(UserControl tmpUI, ref ToolStripPanel tmpPanel)
        {
            foreach (QProcess proc in Global.Processes)
                if (proc.UI.Tag == tmpUI.Tag)
                {
                    foreach (ToolStrip tmpStrip in tmpPanel.Controls)
                        if (tmpStrip.Tag != proc.Menu.Tag)
                            tmpPanel.Controls.Remove(tmpStrip);
                    tmpPanel.Controls.Add(proc.Menu);
                }
        }
    }
}
