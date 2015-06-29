using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QLite.Controls;
using QLite.Structures;

namespace QLite.Objects.QTextSupport
{
    internal class ClearSelectedCommand : UndoableCommand
    {
        string deletedText;

        public ClearSelectedCommand(QText tb)
            : base(tb)
        {
        }

        public override void Undo()
        {
            tb.Selection.Start = new Place(sel.FromX, Math.Min(sel.Start.iLine, sel.End.iLine));
            tb.OnTextChanging();
            InsertTextCommand.InsertText(deletedText, tb);
            tb.OnTextChanged(sel.Start.iLine, sel.End.iLine);
        }

        public override void Execute()
        {
            tb.OnTextChanging();
            deletedText = tb.Selection.Text;
            ClearSelected(tb);
            lastSel = tb.Selection.Clone();
            tb.OnTextChanged(lastSel.Start.iLine, lastSel.Start.iLine);
        }

        internal static void ClearSelected(QText tb)
        {
            Place start = tb.Selection.Start;
            Place end = tb.Selection.End;
            int fromLine = Math.Min(end.iLine, start.iLine);
            int toLine = Math.Max(end.iLine, start.iLine);
            int fromChar = tb.Selection.FromX;
            int toChar = tb.Selection.ToX;
            if (fromLine < 0) return;
            //
            if (fromLine == toLine)
                tb[fromLine].RemoveRange(fromChar, toChar - fromChar);
            else
            {
                tb[fromLine].RemoveRange(fromChar, tb[fromLine].Count - fromChar);
                tb[toLine].RemoveRange(0, toChar);
                tb.RemoveLine(fromLine + 1, toLine - fromLine - 1);
                InsertCharCommand.MergeLines(fromLine, tb);
            }
            //
            tb.Selection.Start = new Place(fromChar, fromLine);
            //
            tb.needRecalc = true;
        }
    }

}
