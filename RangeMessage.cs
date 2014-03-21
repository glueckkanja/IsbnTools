using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace IsbnTools
{
    public class RangeMessage
    {
        private readonly Dictionary<string, RegistrationGroup> _groups = new Dictionary<string, RegistrationGroup>();
        private readonly Dictionary<string, UccPrefix> _uccPrefixes = new Dictionary<string, UccPrefix>();
        private readonly XDocument _xml;

        public RangeMessage(XDocument xml)
        {
            _xml = xml;
            ParseRangeMessage();
        }

        protected internal XDocument RangeMessageXml
        {
            get { return _xml; }
        }

        private void ParseRangeMessage()
        {
            XElement elRoot = _xml.Element("ISBNRangeMessage");

            IEnumerable<XElement> elPrefixes = elRoot.Element("EAN.UCCPrefixes").Elements("EAN.UCC");

            foreach (XElement elPrefix in elPrefixes)
            {
                UccPrefix ucc = UccPrefix.FromXml(elPrefix);
                _uccPrefixes.Add(ucc.Prefix, ucc);
            }

            IEnumerable<XElement> elGroups = elRoot.Element("RegistrationGroups").Elements("Group");

            foreach (XElement elGroup in elGroups)
            {
                RegistrationGroup group = RegistrationGroup.FromXml(elGroup);
                _groups.Add(group.UccPrefix + group.GroupIdentifier, group);
            }
        }

        public UccPrefix FindUccPrefix(string ean)
        {
            string key = ean.Substring(0, 3);

            UccPrefix prefix;
            return _uccPrefixes.TryGetValue(key, out prefix) ? prefix : null;
        }

        public RegistrationGroup FindGroup(string ean)
        {
            string uccPrefix = ean.Substring(0, 3);

            for (int i = 1; i <= 5; i++)
            {
                string groupIdentifier = ean.Substring(3, i);
                string key = uccPrefix + groupIdentifier;

                RegistrationGroup group;
                if (_groups.TryGetValue(key, out group))
                {
                    return @group;
                }
            }

            return null;
        }
    }
}