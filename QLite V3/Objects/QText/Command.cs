using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QLite.Controls;

namespace QLite.Objects.QTextSupport
{
    internal abstract class Command
    {
        internal QText tb;
        public abstract void Execute();
    }

}
