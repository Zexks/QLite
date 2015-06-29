using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QLite.Controls
{
    class QTabs2 : TabControl
    {
        public QTabs2()
            : base()
        {

        }

        public void Add(UserControl control, int procNum)
        {
            TabPage tmp = new TabPage(control.Name);
            control.Dock = DockStyle.Fill;
            tmp.Controls.Add(control);
            tmp.Tag = procNum;
            TabPages.Add(tmp);
        }

    }
}
