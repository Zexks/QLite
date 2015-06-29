using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QLite.Controls;

namespace QLite.Objects.QTextSupport
{
    internal abstract class UndoableCommand : Command
    {
        internal Range sel;
        internal Range lastSel;
        internal bool autoUndo;

        public UndoableCommand(QText tb)
        {
            this.tb = tb;
            sel = tb.Selection.Clone();
        }

        public virtual void Undo()
        {
            OnTextChanged(true);
        }

        public override void Execute()
        {
            lastSel = tb.Selection.Clone();
            OnTextChanged(false);
        }

        protected virtual void OnTextChanged(bool invert)
        {
            bool b = sel.Start.iLine < lastSel.Start.iLine;
            if (invert)
            {
                if (b) tb.OnTextChanged(sel.Start.iLine, sel.Start.iLine);
                else tb.OnTextChanged(sel.Start.iLine, lastSel.Start.iLine);
            }
            else
            {
                if (b) tb.OnTextChanged(sel.Start.iLine, lastSel.Start.iLine);
                else tb.OnTextChanged(lastSel.Start.iLine, lastSel.Start.iLine);
            }
        }
    }

}
