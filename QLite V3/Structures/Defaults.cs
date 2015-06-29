using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace QLite.Structures
{
    public struct Defaults
    {
        public XmlDocument XmlDefaults;
        public string XmlFileName;
        public string PluginPath;
        public bool PasswordLock;
        public int DebugLvl;
        public string[] Connection;
        public char[] InvalidNameChars;
        public string GetInvalidCharString
        {
            get
            {
                string charString = "";
                foreach (char c in InvalidNameChars)
                    charString = charString + ", " + charString.ToString();
                return charString;
            }
        }

    }
}
