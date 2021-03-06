﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace QLite.Objects.VisualMarkers
{
    public class ExpandFoldingMarker : VisualMarker
    {
        public readonly int iLine;

        public ExpandFoldingMarker(int iLine, Rectangle rectangle)
            : base(rectangle)
        {
            this.iLine = iLine;
        }

        public override void Draw(Graphics gr, Pen pen)
        {
            //draw plus
            gr.FillRectangle(Brushes.White, rectangle);
            gr.DrawRectangle(pen, rectangle);
            gr.DrawLine(Pens.Red, rectangle.Left + 2, rectangle.Top + rectangle.Height / 2, rectangle.Right - 2, rectangle.Top + rectangle.Height / 2);
            gr.DrawLine(Pens.Red, rectangle.Left + rectangle.Width / 2, rectangle.Top + 2, rectangle.Left + rectangle.Width / 2, rectangle.Bottom - 2);
        }
    }

}
