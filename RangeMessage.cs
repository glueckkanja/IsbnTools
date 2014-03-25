using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace IsbnTools
{
    public class RangeMessage
    {
        private readonly Dictionary<string, RegistrationGroup> _groups = new Dictionary<string, RegistrationGroup>();
        private readonly int _longestGroup;
        private readonly Dictionary<string, UccPrefix> _uccPrefixes = new Dictionary<string, UccPrefix>();

        public RangeMessage(XDocument xml)
        {
            foreach (XElement el in xml.XPathSelectElements("/ISBNRangeMessage/EAN.UCCPrefixes/EAN.UCC"))
            {
                UccPrefix ucc = UccPrefix.FromXml(el);
                _uccPrefixes.Add(ucc.Prefix, ucc);
            }

            foreach (XElement el in xml.XPathSelectElements("/ISBNRangeMessage/RegistrationGroups/Group"))
            {
                RegistrationGroup group = RegistrationGroup.FromXml(el, this);
                _groups.Add(group.UccPrefix + group.GroupIdentifier, group);
            }

            _longestGroup = _groups.Keys.Max(x => x.Length);
        }

        public UccPrefix FindUccPrefix(string ean)
        {
            if (ean == null || ean.Length < 3) return null;

            if (ean.Length > 3)
                ean = ean.Substring(0, 3);

            UccPrefix prefix;
            if (_uccPrefixes.TryGetValue(ean, out prefix))
            {
                return prefix;
            }

            return null;
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