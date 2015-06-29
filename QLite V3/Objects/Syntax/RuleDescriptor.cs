using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using QLite.Objects.QTextSupport;

namespace QLite.Objects.Syntax
{
    public class RuleDesc
    {
        Regex regex;
        public string pattern;
        public RegexOptions options = RegexOptions.None;
        public Style style;

        public Regex Regex
        {
            get
            {
                if (regex == null)
                {
                    regex = new Regex(pattern, RegexOptions.Compiled | options);
                }
                return regex;
            }
        }
    }

}
