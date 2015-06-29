using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QLite.Controls;
using QLite.Enumerations;
using QLite.Structures;

namespace QLite.Objects.QTextSupport
{
    internal class InsertCharCommand : UndoableCommand
    {
        char c;
        char deletedChar = '\x0';

        public InsertCharCommand(QText tb, char c)
            : base(tb)
        {
            this.c = c;
        }

        public override void Undo()
        {
            tb.OnTextChanging();
            switch (c)
            {
                case '\n': MergeLines(sel.Start.iLine, tb); break;
                case '\r': break;
                case '\b':
                    tb.Selection.Start = lastSel.Start;
                    char cc = '\x0';
                    if (deletedChar != '\x0')
                    {
                        tb.ExpandBlock(tb.Selection.Start.iLine);
                        InsertChar(deletedChar, ref cc, tb);
                    }
                    break;
                default:
                    tb.ExpandBlock(sel.Start.iLine);
                    tb[sel.Start.iLine].RemoveAt(sel.Start.iChar);
                    tb.Selection.Start = sel.Start;
                    break;
            }

            tb.needRecalc = true;

            base.Undo();
        }

        public override void Execute()
        {
            tb.ExpandBlock(tb.Selection.Start.iLine);
            string s = c.ToString();
            tb.OnTextChanging(ref s);
            if (s.Length == 1) c = s[0];

            if (tb.LinesCount == 0) InsertLine(tb);
            InsertChar(c, ref deletedChar, tb);
            tb.needRecalc = true;
            base.Execute();
        }

        internal static void InsertChar(char c, ref char deletedChar, QText tb)
        {
            switch (c)
            {
                case '\n':
                    if (tb.LinesCount == 0) InsertLine(tb);
                    InsertLine(tb);
                    break;
                case '\r': break;
                case '\b'://backspace
                    if (tb.Selection.Start.iChar == 0 && tb.Selection.Start.iLine == 0) return;
                    if (tb.Selection.Start.iChar == 0)
                    {
                        if (tb[tb.Selection.Start.iLine - 1].VisibleState != VisibleState.Visible) tb.ExpandBlock(tb.Selection.Start.iLine - 1);
                        deletedChar = '\n';
                        MergeLines(tb.Selection.Start.iLine - 1, tb);
                    }
                    else
                    {
                        deletedChar = tb[tb.Selection.Start.iLine][tb.Selection.Start.iChar - 1].c;
                        tb[tb.Selection.Start.iLine].RemoveAt(tb.Selection.Start.iChar - 1);
                        tb.Selection.Start = new Place(tb.Selection.Start.iChar - 1, tb.Selection.Start.iLine);
                    }
                    break;
                default:
                    tb[tb.Selection.Start.iLine].Insert(tb.Selection.Start.iChar, new Char(c));
                    tb.Selection.Start = new Place(tb.Selection.Start.iChar + 1, tb.Selection.Start.iLine);
                    break;
            }
        }

        internal static void InsertLine(QText tb)
        {
            if (tb.LinesCount == 0) tb.InsertLine(tb.Selection.Start.iLine + 1, new Line(tb.GenerateUniqueLineId()));
            else BreakLines(tb.Selection.Start.iLine, tb.Selection.Start.iChar, tb);

            tb.Selection.Start = new Place(0, tb.Selection.Start.iLine + 1);
            tb.needRecalc = true;
        }

        internal static void MergeLines(int i, QText tb)
        {
            if (i + 1 >= tb.LinesCount) return;

            tb.ExpandBlock(i);
            tb.ExpandBlock(i + 1);
            int pos = tb[i].Count;
            //
            if (tb[i].Count == 0)
                tb.RemoveLine(i);
            else
                if (tb[i + 1].Count == 0)
                    tb.RemoveLine(i + 1);
                else
                {
                    tb[i].AddRange(tb[i + 1]);
                    tb.RemoveLine(i + 1);
                }
            tb.Selection.Start = new Place(pos, i);
            tb.needRecalc = true;
        }

        internal static void BreakLines(int iLine, int pos, QText tb)
        {
            Line newLine = new Line(tb.GenerateUniqueLineId());
            for (int i = pos; i < tb[iLine].Count; i++)
                newLine.Add(tb[iLine][i]);
            tb[iLine].RemoveRange(pos, tb[iLine].Count - pos);
            tb.InsertLine(iLine + 1, newLine);
        }
    }

}
