using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QLite.Plugins
{
    public interface IPluginHost
    {
        bool Register(IPlugin ipi);
    }

}
