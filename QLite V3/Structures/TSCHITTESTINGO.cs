using System;
using System.Drawing;
using System.Runtime.InteropServices;
using QLite.Enumerations;

namespace QLite.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    public struct TCHITTESTINFO
    {

        public TCHITTESTINFO(Point location)
        {
            pt = location;
            flags = TCHITTESTFLAGS.TCHT_ONITEM;
        }

        public Point pt;
        public TCHITTESTFLAGS flags;
    }
}
