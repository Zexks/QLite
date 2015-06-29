using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QLite.Controls;

namespace QLite.Objects.QTextSupport
{
    internal class ReplaceTextCommand : UndoableCommand
    {
        string insertedText;
        List<Range> ranges;
        string prevText;

        public ReplaceTextCommand(QText tb, List<Range> ranges, string insertedText)
            : base(tb)
        {
            this.ranges = ranges;
            this.insertedText = insertedText;
            sel = tb.Selection.Clone();
            sel.SelectAll();
        }

        public override void Undo()
        {
            tb.Text = prevText;
        }

        public override void Execute()
        {
            tb.OnTextChanging(ref insertedText);

            this.prevText = tb.Text;

            tb.Selection.BeginUpdate();
            for (int i = ranges.Count - 1; i >= 0; i--)
            {
                tb.Selection.Start = ranges[i].Start;
                tb.Selection.End = ranges[i].End;
                ClearSelectedCommand.ClearSelected(tb);
                InsertTextCommand.InsertText(insertedText, tb);
            }
            tb.Selection.SelectAll();
            tb.Selection.EndUpdate();
            tb.needRecalc = true;

            lastSel = tb.Selection.Clone();
            tb.OnTextChanged(lastSel.Start.iLine, lastSel.End.iLine);
            //base.Execute();
        }
    }

}
