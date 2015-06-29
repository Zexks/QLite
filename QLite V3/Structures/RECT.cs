using System;
using System.Drawing;
using System.Runtime.InteropServices;
using QLite.Enumerations;
using QLite.Structures;

namespace QLite.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;

        public RECT(int left, int top, int right, int bottom)
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }

        public RECT(Rectangle r)
        {
            this.left = r.Left;
            this.top = r.Top;
            this.right = r.Right;
            this.bottom = r.Bottom;
        }

        public static RECT FromXYWH(int x, int y, int width, int height)
        {
            return new RECT(x, y, x + width, y + height);
        }

        public static RECT FromIntPtr(IntPtr ptr)
        {
            RECT rect = (RECT)Marshal.PtrToStructure(ptr, typeof(RECT));
            return rect;
        }

        public Size Size
        {
            get
            {
                return new Size(this.right - this.left, this.bottom - this.top);
            }
        }
    }

}
