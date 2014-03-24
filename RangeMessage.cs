using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace IsbnTools
{
    public class RangeMessage
    {
        private readonly Dictionary<string, RegistrationGroup> _groups = new Dictionary<string, RegistrationGroup>();
        private readonly Dictionary<string, UccPrefix> _uccPrefixes = new Dictionary<string, UccPrefix>();
        private readonly XDocument _xml;
        private int _longestGroup;

        public RangeMessage(XDocument xml)
        {
            _xml = xml;
            ParseRangeMessage();
        }

        internal XDocument RangeMessageXml
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
                RegistrationGroup group = RegistrationGroup.FromXml(elGroup, this);
                _groups.Add(group.UccPrefix + group.GroupIdentifier, group);
            }

            _longestGroup = _groups.Max(x => x.Key.Length);
        }

        public UccPrefix FindUccPrefix(string ean)
        {
            string key = ean.Substring(0, 3);

            UccPrefix prefix;
            return _uccPrefixes.TryGetValue(key, out prefix) ? prefix : null;
        }

        public RegistrationGroup FindGroup(string ean)
        {
            for (int i = 4; i <= _longestGroup; i++)
            {
                string key = ean.Substring(0, i);

                RegistrationGroup group;
                if (_groups.TryGetValue(key, out group))
                {
                    return group;
                }
            }

            return null;
        }
    }
}