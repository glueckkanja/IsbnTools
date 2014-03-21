using System;

namespace IsbnTools
{
    public class Isbn13 : Ean13
    {
        public Isbn13(string uccPrefix, string groupIdentifier, string publisher, string title)
            : this(
                uccPrefix, groupIdentifier, publisher, title,
                CalculateEan13CheckDigit(uccPrefix + groupIdentifier + publisher + title))
        {
        }

        public Isbn13(string uccPrefix, string groupIdentifier, string publisher, string title, int checkDigit)
            : base(uccPrefix + groupIdentifier + publisher + title, checkDigit)
        {
            UccPrefix = uccPrefix;
            GroupIdentifier = groupIdentifier;
            Publisher = publisher;
            Title = title;
            CheckDigit = checkDigit;
        }

        public string UccPrefix { get; protected set; }
        public string GroupIdentifier { get; protected set; }
        public string Publisher { get; protected set; }
        public string Title { get; protected set; }

        public override string ToString()
        {
            return ToString("-");
        }

        protected override string ToString(string seperator)
        {
            return string.Concat(new object[]
            {
                UccPrefix,
                seperator,
                GroupIdentifier,
                seperator,
                Publisher,
                seperator,
                Title,
                seperator,
                base.CheckDigit
            });
        }
    }
}