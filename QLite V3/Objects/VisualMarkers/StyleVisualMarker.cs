using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using QLite.Objects.QTextSupport;

namespace QLite.Objects.VisualMarkers
{
    public class StyleVisualMarker : VisualMarker
    {
        public Style Style { get; private set; }

        public StyleVisualMarker(Rectangle rectangle, Style style)
            : base(rectangle)
        {
            this.Style = style;
        }
    }

}
