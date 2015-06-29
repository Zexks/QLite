using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QLite.Events
{
    public delegate bool PreRemoveTab(int indx);
    public delegate void PageUpdated(object sender, PluginEventArgs e);
    public delegate void ExecuteQuery(object sender, EventArgs e);
    public delegate void CancelQuery(object sender, EventArgs e);
    public delegate void QuerySelected(object sender, EventArgs e);
    public delegate void DatabaseSelected(object sender, EventArgs e);
    public delegate void PageAdded(object sender, ControlEventArgs cea);
    public delegate void PageRemoved(object sender, ControlEventArgs cea);
    public delegate void PageChanged(object sender, ControlEventArgs e);
    public delegate void TabChangingEventHandler(object sender, TabChangingEventArgs tcea);

}
