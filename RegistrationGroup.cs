﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace IsbnTools
{
    public class RegistrationGroup
    {
        private RegistrationGroup(UccPrefix uccPrefix, string groupIdentifier, string agency, ICollection<Range> rules)
        {
            UccPrefix = uccPrefix;
            GroupIdentifier = groupIdentifier;
            Agency = agency;
            Rules = rules;
        }

        public UccPrefix UccPrefix { get; private set; }
        public string GroupIdentifier { get; private set; }
        public string Agency { get; private set; }
        public ICollection<Range> Rules { get; private set; }

        public static RegistrationGroup FromXml(XElement groupElement, RangeMessage rangeMessage)
        {
            var prefix = (string) groupElement.Element("Prefix");
            var agency = (string) groupElement.Element("Agency");

            string[] parts = prefix.Split('-');

            string uccPrefix = parts[0];
            string groupIdentifier = parts[1];

            List<Range> rules = groupElement.Element("Rules").Elements("Rule").Select(Range.FromXml).ToList();

            return new RegistrationGroup(rangeMessage.FindUccPrefix(uccPrefix), groupIdentifier, agency, rules);
        }

        public override string ToString()
        {
            return UccPrefix + GroupIdentifier;
        }
    }
}