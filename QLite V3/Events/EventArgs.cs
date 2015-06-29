using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using QLite.Controls;
using QLite.Objects.AutoComplete;
using QLite.Objects.QTextSupport;
using QLite.Objects.VisualMarkers;

namespace QLite.Events
{
    public class LineRemovedEventArgs : EventArgs
    {
        public int Index { get; private set; }
        public int Count { get; private set; }
        public List<int> RemovedLineUniqueIds { get; private set; }

        public LineRemovedEventArgs(int index, int count, List<int> removedLineIds)
        {
            this.Index = index;
            this.Count = count;
            this.RemovedLineUniqueIds = removedLineIds;
        }
    }

    public class LineInsertedEventArgs : EventArgs
    {
        public int Index { get; private set; }
        public int Count { get; private set; }

        public LineInsertedEventArgs(int index, int count)
        {
            this.Index = index;
            this.Count = count;
        }

    }

    public class AutoIndentEventArgs : EventArgs
    {
        public int iLine { get; internal set; }
        public int TabLength { get; internal set; }
        public string LineText { get; internal set; }
        public string PrevLineText { get; internal set; }
        /// <summary>
        /// Additional spaces count for this line, relative to previous line
        /// </summary>
        public int Shift { get; set; }
        /// <summary>
        /// Additional spaces count for next line, relative to previous line
        /// </summary>
        public int ShiftNextLines { get; set; }

        public AutoIndentEventArgs(int iLine, string lineText, string prevLineText, int tabLength)
        {
            this.iLine = iLine;
            this.LineText = lineText;
            this.PrevLineText = prevLineText;
            this.TabLength = tabLength;
        }
    }

    public class PaintLineEventArgs : PaintEventArgs
    {
        public int LineIndex { get; private set; }
        public Rectangle LineRect { get; private set; }

        public PaintLineEventArgs(int iLine, Rectangle rect, Graphics gr, Rectangle clipRect)
            : base(gr, clipRect)
        {
            LineIndex = iLine;
            LineRect = rect;
        }
    }

    public class PluginEventArgs : EventArgs
    {
        public string Message { get; private set; }
        public EventArgs Args { get; private set; }

        public PluginEventArgs(string msg, EventArgs e)
        {
            Message = msg;
            Args = e;
        }
    }

    public class VisualMarkerEventArgs : MouseEventArgs
    {
        public Style Style { get; private set; }
        public StyleVisualMarker Marker { get; private set; }

        public VisualMarkerEventArgs(Style style, StyleVisualMarker marker, MouseEventArgs args)
            : base(args.Button, args.Clicks, args.X, args.Y, args.Delta)
        {
            this.Style = style;
            this.Marker = marker;
        }
    }

    public class TextChangingEventArgs : EventArgs
    {
        public string InsertingText { get; set; }
    }

    public class TextChangedEventArgs : EventArgs
    {
        public Range ChangedRange { get; set; }

        public TextChangedEventArgs(Range changedRange)
        {
            this.ChangedRange = changedRange;
        }
    }

    public class TabChangingEventArgs : EventArgs
    {
        public TabChangingEventArgs(int currentIndex, int newIndex)
        {
            this.cIndex = currentIndex;
            this.nIndex = newIndex;
        }

        public int CurrentIndex
        { get { return cIndex; } }

        public int NewIndex
        { get { return nIndex; } }

        public bool Cancel
        { 
            get { return cancelEvent; }
            set { cancelEvent = value; }
        }

        private bool cancelEvent;
        private int cIndex;
        private int nIndex;
    }

    public class SelectingEventArgs : EventArgs
    {
        public AutoCompleteItem Item { get; internal set; }
        public bool Cancel { get; set; }
        public int SelectedIndex { get; set; }
        public bool Handled { get; set; }
    }

    public class SelectedEventArgs : EventArgs
    {
        public AutoCompleteItem Item { get; internal set; }
        public QText Tb { get; set; }
    }
}