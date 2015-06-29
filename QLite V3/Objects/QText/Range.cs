using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using QLite.Enumerations;
using QLite.Controls;
using QLite.Structures;

namespace QLite.Objects.QTextSupport
{
    public class Range : IEnumerable<Place>
    {
        Place start;
        Place end;
        public readonly QText tb;
        int preferedPos = -1;
        int updating = 0;

        string cachedText;
        List<Place> cachedCharIndexToPlace;
        int cachedTextVersion = -1;

        #region Constructors
        public Range(QText tb)
        {
            this.tb = tb;
        }

        public Range(QText tb, Place start, Place end)
            : this(tb)
        {
            this.start = start;
            this.end = end;
        }

        public Range(QText tb, int iStartChar, int iStartLine, int iEndChar, int iEndLine)
            : this(tb)
        {
            start = new Place(iStartChar, iStartLine);
            end = new Place(iEndChar, iEndLine);
        }

        #endregion

        public bool Contains(Place place)
        {
            if (place.iLine < Math.Min(start.iLine, end.iLine)) return false;
            if (place.iLine > Math.Max(start.iLine, end.iLine)) return false;

            Place s = start, e = end;

            if (s.iLine > e.iLine || (s.iLine == e.iLine && s.iChar > e.iChar))
            {
                var temp = s;
                s = e;
                e = temp;
            }

            if (place.iLine == s.iLine && place.iChar < s.iChar) return false;
            if (place.iLine == e.iLine && place.iChar > e.iChar) return false;

            return true;
        }

        public Range GetIntersectionWith(Range range)
        {
            Range r1 = this.Clone();
            Range r2 = range.Clone();
            r1.Normalize();
            r2.Normalize();
            Place newStart = r1.Start > r2.Start ? r1.Start : r2.Start;
            Place newEnd = r1.End < r2.End ? r1.End : r2.End;
            if (newEnd < newStart)
                return new Range(tb, start, start);
            return tb.GetRange(newStart, newEnd);
        }

        public Place Start
        {
            get { return start; }
            set
            {
                end = start = value;
                preferedPos = -1;

                OnSelectionChanged();
            }
        }

        public Place End
        {
            get
            {
                return end;
            }
            set
            {
                end = value;
                OnSelectionChanged();
            }
        }

        public string Text
        {
            get
            {
                int fromLine = Math.Min(end.iLine, start.iLine);
                int toLine = Math.Max(end.iLine, start.iLine);
                int fromChar = FromX;
                int toChar = ToX;
                if (fromLine < 0) return null;
                //
                StringBuilder sb = new StringBuilder();
                for (int y = fromLine; y <= toLine; y++)
                {
                    int fromX = y == fromLine ? fromChar : 0;
                    int toX = y == toLine ? toChar - 1 : tb[y].Count - 1;
                    for (int x = fromX; x <= toX; x++)
                        sb.Append(tb[y][x].c);
                    if (y != toLine && fromLine != toLine)
                        sb.AppendLine();
                }
                return sb.ToString();
            }
        }

        public char CharAfterStart
        {
            get
            {
                if (Start.iChar >= tb[Start.iLine].Count)
                    return '\n';
                else
                    return tb[Start.iLine][Start.iChar].c;
            }
        }

        public char CharBeforeStart
        {
            get
            {
                if (Start.iChar <= 0)
                    return '\n';
                else
                    return tb[Start.iLine][Start.iChar - 1].c;
            }
        }

        internal int FromX
        {
            get
            {
                if (end.iLine < start.iLine) return end.iChar;
                if (end.iLine > start.iLine) return start.iChar;
                return Math.Min(end.iChar, start.iChar);
            }
        }

        internal int ToX
        {
            get
            {
                if (end.iLine < start.iLine) return start.iChar;
                if (end.iLine > start.iLine) return end.iChar;
                return Math.Max(end.iChar, start.iChar);
            }
        }

        internal void GetText(out string text, out List<Place> charIndexToPlace)
        {
            //try get cached text
            if (tb.TextVersion == cachedTextVersion)
            {
                text = cachedText;
                charIndexToPlace = cachedCharIndexToPlace;
                return;
            }
            //
            int fromLine = Math.Min(end.iLine, start.iLine);
            int toLine = Math.Max(end.iLine, start.iLine);
            int fromChar = FromX;
            int toChar = ToX;

            StringBuilder sb = new StringBuilder((toLine - fromLine) * 100);
            charIndexToPlace = new List<Place>(sb.Capacity);
            if (fromLine >= 0)
            {
                for (int y = fromLine; y <= toLine; y++)
                {
                    int fromX = y == fromLine ? fromChar : 0;
                    int toX = y == toLine ? toChar - 1 : tb[y].Count - 1;
                    for (int x = fromX; x <= toX; x++)
                    {
                        sb.Append(tb[y][x].c);
                        charIndexToPlace.Add(new Place(x, y));
                    }
                    if (y != toLine && fromLine != toLine)
                        foreach (char c in Environment.NewLine)
                        {
                            sb.Append(c);
                            charIndexToPlace.Add(new Place(tb[y].Count/*???*/, y));
                        }
                }
            }
            text = sb.ToString();
            charIndexToPlace.Add(End > Start ? End : Start);
            //caching
            cachedText = text;
            cachedCharIndexToPlace = charIndexToPlace;
            cachedTextVersion = tb.TextVersion;
        }

        public Range Clone()
        {
            return (Range)MemberwiseClone();
        }

        #region Movement
        public bool GoRight()
        {
            Place prevStart = start;
            GoRight(false);
            return prevStart != start;
        }

        public bool GoRightThroughFolded()
        {
            if (start.iLine >= tb.LinesCount - 1 && start.iChar >= tb[tb.LinesCount - 1].Count)
                return false;

            if (start.iChar < tb[start.iLine].Count)
                start.Offset(1, 0);
            else
                start = new Place(0, start.iLine + 1);

            preferedPos = -1;
            end = start;
            OnSelectionChanged();
            return true;
        }

        public bool GoLeft()
        {
            Place prevStart = start;
            GoLeft(false);
            return prevStart != start;
        }

        public bool GoLeftThroughFolded()
        {
            if (start.iChar == 0 && start.iLine == 0)
                return false;

            if (start.iChar > 0)
                start.Offset(-1, 0);
            else
                start = new Place(tb[start.iLine - 1].Count, start.iLine - 1);

            preferedPos = -1;
            end = start;
            OnSelectionChanged();
            return true;
        }

        public void GoLeft(bool shift)
        {
            if (start.iChar != 0 || start.iLine != 0)
            {
                if (start.iChar > 0 && tb[start.iLine].VisibleState == VisibleState.Visible)
                    start.Offset(-1, 0);
                else
                {
                    int i = tb.FindPrevVisibleLine(start.iLine);
                    if (i == start.iLine) return;
                    start = new Place(tb[i].Count, i);
                }
            }

            if (!shift)
                end = start;

            OnSelectionChanged();

            preferedPos = -1;
        }

        public void GoRight(bool shift)
        {
            if (start.iLine < tb.LinesCount - 1 || start.iChar < tb[tb.LinesCount - 1].Count)
            {
                if (start.iChar < tb[start.iLine].Count && tb[start.iLine].VisibleState == VisibleState.Visible)
                    start.Offset(1, 0);
                else
                {
                    int i = tb.FindNextVisibleLine(start.iLine);
                    if (i == start.iLine) return;
                    start = new Place(0, i);
                }
            }

            if (!shift)
                end = start;

            OnSelectionChanged();

            preferedPos = -1;
        }

        internal void GoUp(bool shift)
        {
            if (preferedPos < 0)
                preferedPos = start.iChar - tb[start.iLine].GetWordWrapStringStartPosition(tb[start.iLine].GetWordWrapStringIndex(start.iChar));

            int iWW = tb[start.iLine].GetWordWrapStringIndex(start.iChar);
            if (iWW == 0)
            {
                if (start.iLine <= 0) return;
                int i = tb.FindPrevVisibleLine(start.iLine);
                if (i == start.iLine) return;
                start.iLine = i;
                iWW = tb[start.iLine].WordWrapStringsCount;
            }

            if (iWW > 0)
            {
                int finish = tb[start.iLine].GetWordWrapStringFinishPosition(iWW - 1);
                start.iChar = tb[start.iLine].GetWordWrapStringStartPosition(iWW - 1) + preferedPos;
                if (start.iChar > finish + 1)
                    start.iChar = finish + 1;
            }

            if (!shift)
                end = start;

            OnSelectionChanged();
        }

        internal void GoPageUp(bool shift)
        {
            if (preferedPos < 0)
                preferedPos = start.iChar - tb[start.iLine].GetWordWrapStringStartPosition(tb[start.iLine].GetWordWrapStringIndex(start.iChar));

            int pageHeight = tb.ClientRectangle.Height / tb.CharHeight - 1;

            for (int i = 0; i < pageHeight; i++)
            {
                int iWW = tb[start.iLine].GetWordWrapStringIndex(start.iChar);
                if (iWW == 0)
                {
                    if (start.iLine <= 0) break;
                    //pass hidden
                    int newLine = tb.FindPrevVisibleLine(start.iLine);
                    if (newLine == start.iLine) break;
                    start.iLine = newLine;
                    iWW = tb[start.iLine].WordWrapStringsCount;
                }

                if (iWW > 0)
                {
                    int finish = tb[start.iLine].GetWordWrapStringFinishPosition(iWW - 1);
                    start.iChar = tb[start.iLine].GetWordWrapStringStartPosition(iWW - 1) + preferedPos;
                    if (start.iChar > finish + 1)
                        start.iChar = finish + 1;
                }
            }

            if (!shift)
                end = start;

            OnSelectionChanged();
        }

        internal void GoDown(bool shift)
        {
            if (preferedPos < 0)
                preferedPos = start.iChar - tb[start.iLine].GetWordWrapStringStartPosition(tb[start.iLine].GetWordWrapStringIndex(start.iChar));

            int iWW = tb[start.iLine].GetWordWrapStringIndex(start.iChar);
            if (iWW >= tb[start.iLine].WordWrapStringsCount - 1)
            {
                if (start.iLine >= tb.LinesCount - 1) return;
                //pass hidden
                int i = tb.FindNextVisibleLine(start.iLine);
                if (i == start.iLine) return;
                start.iLine = i;
                iWW = -1;
            }

            if (iWW < tb[start.iLine].WordWrapStringsCount - 1)
            {
                int finish = tb[start.iLine].GetWordWrapStringFinishPosition(iWW + 1);
                start.iChar = tb[start.iLine].GetWordWrapStringStartPosition(iWW + 1) + preferedPos;
                if (start.iChar > finish + 1)
                    start.iChar = finish + 1;
            }

            if (!shift)
                end = start;

            OnSelectionChanged();
        }

        internal void GoPageDown(bool shift)
        {
            if (preferedPos < 0)
                preferedPos = start.iChar - tb[start.iLine].GetWordWrapStringStartPosition(tb[start.iLine].GetWordWrapStringIndex(start.iChar));

            int pageHeight = tb.ClientRectangle.Height / tb.CharHeight - 1;

            for (int i = 0; i < pageHeight; i++)
            {
                int iWW = tb[start.iLine].GetWordWrapStringIndex(start.iChar);
                if (iWW >= tb[start.iLine].WordWrapStringsCount - 1)
                {
                    if (start.iLine >= tb.LinesCount - 1) break;
                    //pass hidden
                    int newLine = tb.FindNextVisibleLine(start.iLine);
                    if (newLine == start.iLine) break;
                    start.iLine = newLine;
                    iWW = -1;
                }

                if (iWW < tb[start.iLine].WordWrapStringsCount - 1)
                {
                    int finish = tb[start.iLine].GetWordWrapStringFinishPosition(iWW + 1);
                    start.iChar = tb[start.iLine].GetWordWrapStringStartPosition(iWW + 1) + preferedPos;
                    if (start.iChar > finish + 1)
                        start.iChar = finish + 1;
                }
            }

            if (!shift)
                end = start;

            OnSelectionChanged();
        }

        internal void GoHome(bool shift)
        {
            if (start.iLine < 0)
                return;

            if (tb[start.iLine].VisibleState != VisibleState.Visible)
                return;

            start = new Place(0, start.iLine);

            if (!shift)
                end = start;

            OnSelectionChanged();

            preferedPos = -1;
        }

        internal void GoEnd(bool shift)
        {
            if (start.iLine < 0)
                return;
            if (tb[start.iLine].VisibleState != VisibleState.Visible)
                return;

            start = new Place(tb[start.iLine].Count, start.iLine);

            if (!shift)
                end = start;

            OnSelectionChanged();

            preferedPos = -1;
        }

        internal void GoWordLeft(bool shift)
        {
            Range range = this.Clone();//for OnSelectionChanged disable

            Place prev;
            bool findIdentifier = IsIdentifierChar(range.CharBeforeStart);

            do
            {
                prev = range.Start;
                if (IsIdentifierChar(range.CharBeforeStart) ^ findIdentifier)
                    break;

                //move left
                range.GoLeft(shift);
            } while (prev != range.Start);

            this.Start = range.Start;
            this.End = range.End;

            if (tb[Start.iLine].VisibleState != VisibleState.Visible)
                GoRight(shift);
        }

        internal void GoWordRight(bool shift)
        {
            Range range = this.Clone();//for OnSelectionChanged disable

            Place prev;
            bool findIdentifier = IsIdentifierChar(range.CharAfterStart);

            do
            {
                prev = range.Start;
                if (IsIdentifierChar(range.CharAfterStart) ^ findIdentifier)
                    break;

                //move right
                range.GoRight(shift);
            } while (prev != range.Start);

            this.Start = range.Start;
            this.End = range.End;

            if (tb[Start.iLine].VisibleState != VisibleState.Visible)
                GoLeft(shift);
        }

        internal void GoFirst(bool shift)
        {
            start = new Place(0, 0);
            if (tb[Start.iLine].VisibleState != VisibleState.Visible)
                GoRight(shift);
            if (!shift)
                end = start;

            OnSelectionChanged();
        }

        internal void GoLast(bool shift)
        {
            start = new Place(tb[tb.LinesCount - 1].Count, tb.LinesCount - 1);
            if (tb[Start.iLine].VisibleState != VisibleState.Visible)
                GoLeft(shift);
            if (!shift)
                end = start;

            OnSelectionChanged();
        }

        #endregion

        #region Styles
        public void ClearStyle(params Style[] styles)
        {
            try
            {
                ClearStyle(tb.GetStyleIndexMask(styles));
            }
            catch { ;}
        }

        public void ClearStyle(StyleIndex styleIndex)
        {
            //set code to chars
            int fromLine = Math.Min(End.iLine, Start.iLine);
            int toLine = Math.Max(End.iLine, Start.iLine);
            int fromChar = FromX;
            int toChar = ToX;
            if (fromLine < 0) return;
            //
            for (int y = fromLine; y <= toLine; y++)
            {
                int fromX = y == fromLine ? fromChar : 0;
                int toX = y == toLine ? toChar - 1 : tb[y].Count - 1;
                for (int x = fromX; x <= toX; x++)
                {
                    Char c = tb[y][x];
                    c.style &= ~styleIndex;
                    tb[y][x] = c;
                }
            }
            //
            tb.Invalidate();
        }

        public void SetStyle(Style style)
        {
            //search code for style
            int code = tb.GetOrSetStyleLayerIndex(style);
            //set code to chars
            SetStyle(ToStyleIndex(code));
            //
            tb.Invalidate();
        }

        public void SetStyle(Style style, string regexPattern)
        {
            //search code for style
            StyleIndex layer = ToStyleIndex(tb.GetOrSetStyleLayerIndex(style));
            SetStyle(layer, regexPattern, RegexOptions.None);
        }

        public void SetStyle(Style style, Regex regex)
        {
            //search code for style
            StyleIndex layer = ToStyleIndex(tb.GetOrSetStyleLayerIndex(style));
            SetStyle(layer, regex);
        }

        public void SetStyle(Style style, string regexPattern, RegexOptions options)
        {
            //search code for style
            StyleIndex layer = ToStyleIndex(tb.GetOrSetStyleLayerIndex(style));
            SetStyle(layer, regexPattern, options);
        }

        public void SetStyle(StyleIndex styleLayer, string regexPattern, RegexOptions options)
        {
            if (Math.Abs(Start.iLine - End.iLine) > 1000)
                options |= RegexOptions.Compiled;
            //
            foreach (var range in GetRanges(regexPattern, options))
                range.SetStyle(styleLayer);
            //
            tb.Invalidate();
        }

        public void SetStyle(StyleIndex styleLayer, Regex regex)
        {
            foreach (var range in GetRanges(regex))
                range.SetStyle(styleLayer);
            //
            tb.Invalidate();
        }

        public void SetStyle(StyleIndex styleIndex)
        {
            //set code to chars
            int fromLine = Math.Min(End.iLine, Start.iLine);
            int toLine = Math.Max(End.iLine, Start.iLine);
            int fromChar = FromX;
            int toChar = ToX;
            if (fromLine < 0) return;
            //
            for (int y = fromLine; y <= toLine; y++)
            {
                int fromX = y == fromLine ? fromChar : 0;
                int toX = y == toLine ? toChar - 1 : tb[y].Count - 1;
                for (int x = fromX; x <= toX; x++)
                {
                    Char c = tb[y][x];
                    c.style |= styleIndex;
                    tb[y][x] = c;
                }
            }
        }

        #endregion

        #region Set Folding Markers
        public void SetFoldingMarkers(string startFoldingPattern, string finishFoldingPattern)
        {
            SetFoldingMarkers(startFoldingPattern, finishFoldingPattern, RegexOptions.Compiled);
        }

        public void SetFoldingMarkers(string startFoldingPattern, string finishFoldingPattern, RegexOptions options)
        {
            foreach (var range in GetRanges(startFoldingPattern, options))
                tb[range.Start.iLine].FoldingStartMarker = startFoldingPattern;

            foreach (var range in GetRanges(finishFoldingPattern, options))
                tb[range.Start.iLine].FoldingEndMarker = startFoldingPattern;
            //
            tb.Invalidate();
        }

        #endregion

        #region Get Ranges
        public IEnumerable<Range> GetRanges(Regex regex)
        {
            //get text
            string text;
            List<Place> charIndexToPlace;
            GetText(out text, out charIndexToPlace);
            //
            foreach (Match m in regex.Matches(text))
            {
                Range r = new Range(this.tb);
                //try get 'range' group, otherwise use group 0
                Group group = m.Groups["range"];
                if (!group.Success)
                    group = m.Groups[0];
                //
                r.Start = charIndexToPlace[group.Index];
                r.End = charIndexToPlace[group.Index + group.Length];
                yield return r;
            }
        }

        public IEnumerable<Range> GetRanges(string regexPattern)
        {
            return GetRanges(regexPattern, RegexOptions.None);
        }

        public IEnumerable<Range> GetRanges(string regexPattern, RegexOptions options)
        {
            //get text
            string text;
            List<Place> charIndexToPlace;
            GetText(out text, out charIndexToPlace);
            //create regex
            Regex regex = new Regex(regexPattern, options);
            //
            foreach (Match m in regex.Matches(text))
            {
                Range r = new Range(this.tb);
                //try get 'range' group, otherwise use group 0
                Group group = m.Groups["range"];
                if (!group.Success)
                    group = m.Groups[0];
                //
                r.Start = charIndexToPlace[group.Index];
                r.End = charIndexToPlace[group.Index + group.Length];
                yield return r;
            }
        }

        #endregion

        public void ClearFoldingMarkers()
        {
            //set code to chars
            int fromLine = Math.Min(End.iLine, Start.iLine);
            int toLine = Math.Max(End.iLine, Start.iLine);
            if (fromLine < 0) return;
            //
            for (int y = fromLine; y <= toLine; y++)
                tb[y].ClearFoldingMarkers();
            //
            tb.Invalidate();
        }

        void OnSelectionChanged()
        {
            //clear cache
            cachedTextVersion = -1;
            cachedText = null;
            cachedCharIndexToPlace = null;
            //
            if (tb.Selection == this)
                if (updating == 0)
                    tb.OnSelectionChanged();
        }

        public override string ToString()
        {
            return "Start: " + Start + " End: " + End;
        }

        public void SelectAll()
        {
            Start = new Place(0, 0);
            if (tb.LinesCount == 0) Start = new Place(0, 0);
            else
            {
                end = new Place(0, 0);
                start = new Place(tb[tb.LinesCount - 1].Count, tb.LinesCount - 1);
            }
            if (this == tb.Selection) tb.Invalidate();
        }

        public void BeginUpdate()
        {
            updating++;
        }

        public void EndUpdate()
        {
            updating--;
            if (updating == 0)
                OnSelectionChanged();
        }

        public void Normalize()
        {
            if (Start > End)
                Inverse();
        }

        public void Inverse()
        {
            var temp = start;
            start = end;
            end = temp;
        }

        public void Expand()
        {
            Normalize();
            start = new Place(0, start.iLine);
            end = new Place(tb.GetLineLength(end.iLine), end.iLine);
        }

        public Range GetFragment(string allowedSymbolsPattern)
        {
            return GetFragment(allowedSymbolsPattern, RegexOptions.None);
        }

        public Range GetFragment(string allowedSymbolsPattern, RegexOptions options)
        {
            Range r = new Range(tb);
            r.Start = Start;
            Regex regex = new Regex(allowedSymbolsPattern, options);
            //go left, check symbols
            while (r.GoLeftThroughFolded())
                if (!regex.IsMatch(r.CharAfterStart.ToString()))
                { r.GoRightThroughFolded(); break; }

            Place startFragment = r.Start;

            r.Start = Start;
            //go right, check symbols
            do
            {
                if (!regex.IsMatch(r.CharAfterStart.ToString()))
                    break;
            } while (r.GoRightThroughFolded());
            Place endFragment = r.Start;

            return new Range(tb, startFragment, endFragment);
        }

        bool IsIdentifierChar(char c)
        {
            return char.IsLetterOrDigit(c) || c == '_';
        }

        public static StyleIndex ToStyleIndex(int i)
        {
            return (StyleIndex)(1 << i);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (this as IEnumerable<Place>).GetEnumerator();
        }

        IEnumerator<Place> IEnumerable<Place>.GetEnumerator()
        {
            int fromLine = Math.Min(end.iLine, start.iLine);
            int toLine = Math.Max(end.iLine, start.iLine);
            int fromChar = FromX;
            int toChar = ToX;
            if (fromLine < 0) yield break;
            //
            for (int y = fromLine; y <= toLine; y++)
            {
                int fromX = y == fromLine ? fromChar : 0;
                int toX = y == toLine ? toChar - 1 : tb[y].Count - 1;
                for (int x = fromX; x <= toX; x++)
                    yield return new Place(x, y);
            }
        }

    }

}
