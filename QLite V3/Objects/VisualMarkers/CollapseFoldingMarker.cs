using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace QLite.Objects.VisualMarkers
{
    public class CollapseFoldingMarker : VisualMarker
    {
        public readonly int iLine;

        public CollapseFoldingMarker(int iLine, Rectangle rectangle)
            : base(rectangle)
        {
            this.iLine = iLine;
        }

        public override void Draw(Graphics gr, Pen pen)
        {
            //draw minus
            gr.FillRectangle(Brushes.White, rectangle);
            gr.DrawRectangle(pen, rectangle);
            gr.DrawLine(pen, rectangle.Left + 2, rectangle.Top + rectangle.Height / 2, rectangle.Right - 2, rectangle.Top + rectangle.Height / 2);
        }
    }

}
