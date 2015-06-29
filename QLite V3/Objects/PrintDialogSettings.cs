using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QLite.Objects.QTextSupport;

namespace QLite.Objects
{
    public class PrintDialogSettings
    {
        public bool ShowPageSetupDialog = false;
        public bool ShowPrintDialog = false;
        public bool ShowPrintPreviewDialog = true;
        internal Range printRange;
    }

}
