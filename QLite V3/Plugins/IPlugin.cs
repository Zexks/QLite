using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using QLite.Events;

namespace QLite.Plugins
{
    public interface IPlugin
    {
        string Name { get; }
        string[] Info { get; }
        ToolStrip Menu { get; }
        UserControl UI { get; }
        Form Options { get; }
        event PageUpdated OnPageUpdated;
        bool Initialize();
        void Dispose();
    }

}
