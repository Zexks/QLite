using System;
using System.Runtime.InteropServices;
using QLite.Enumerations;
using QLite.Structures;

namespace QLite.Structures
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct PAINTSTRUCT
    {
        public IntPtr hdc;
        public int fErase;
        public RECT rcPaint;
        public int fRestore;
        public int fIncUpdate;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] rgbReserved;
    }
}
