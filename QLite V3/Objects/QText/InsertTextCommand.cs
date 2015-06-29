using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QLite.Controls;

namespace QLite.Objects.QTextSupport
{
    internal class InsertTextCommand : UndoableCommand
    {
        string insertedText;

        public InsertTextCommand(QText tb, string insertedText)
            : base(tb)
        {
            this.insertedText = insertedText;
        }

        public override void Undo()
        {
            tb.Selection.Start = sel.Start;
            tb.Selection.End = lastSel.Start;
            tb.OnTextChanging();
            ClearSelectedCommand.ClearSelected(tb);
            base.Undo();
        }

        public override void Execute()
        {
            tb.OnTextChanging(ref insertedText);
            InsertText(insertedText, tb);
            base.Execute();
        }

        internal static void InsertText(string insertedText, QText tb)
        {
            try
            {
                tb.Selection.BeginUpdate();
                char cc = '\x0';
                if (tb.LinesCount == 0)
                    InsertCharCommand.InsertLine(tb);
                tb.ExpandBlock(tb.Selection.Start.iLine);
                foreach (char c in insertedText)
                    InsertCharCommand.InsertChar(c, ref cc, tb);
                tb.needRecalc = true;
            }
            finally
            {
                tb.Selection.EndUpdate();
            }
        }
    }

}
