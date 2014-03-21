using System;
using System.Xml.Linq;

namespace IsbnTools
{
    public class UccPrefix
    {
        private UccPrefix(string prefix, string agency)
        {
            Prefix = prefix;
            Agency = agency;
        }

        public string Prefix { get; private set; }
        public string Agency { get; private set; }

        public static UccPrefix FromXml(XElement uccElement)
        {
            var prefix = (string) uccElement.Element("Prefix");
            var agency = (string) uccElement.Element("Agency");

            return new UccPrefix(prefix, agency);
        }

        public override string ToString()
        {
            return Prefix;
        }
    }
}