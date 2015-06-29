using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using QLite.Enumerations;

namespace QLite
{
    public struct Char
    {
        public char c;
        //Bit 1 in position n means that this char will rendering by QText.Styles[n]</remarks>
        public StyleIndex style;

        public Char(char c)
        {
            this.c = c;
            style = StyleIndex.None;
        }
    }

}
