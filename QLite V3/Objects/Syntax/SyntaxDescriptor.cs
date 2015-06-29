using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QLite.Objects.QTextSupport;

namespace QLite.Objects.Syntax
{
    public class SyntaxDescriptor : IDisposable
    {
        public char leftBracket = '(';
        public char rightBracket = ')';
        public char leftBracket2 = '\x0';
        public char rightBracket2 = '\x0';
        public readonly List<Style> styles = new List<Style>();
        public readonly List<RuleDesc> rules = new List<RuleDesc>();
        public readonly List<FoldingDesc> foldings = new List<FoldingDesc>();

        public void Dispose()
        {
            foreach (var style in styles)
                style.Dispose();
        }
    }

}
