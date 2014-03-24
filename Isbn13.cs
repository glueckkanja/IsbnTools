using System;

namespace IsbnTools
{
    public class Isbn13 : Ean13
    {
        protected internal Isbn13(RegistrationGroup group, string publisher, string title)
            : this(group, publisher, title, CalculateEan13CheckDigit("" + group + publisher + title))
        {
        }

        protected internal Isbn13(RegistrationGroup group, string publisher, string title, int checkDigit)
            : base(group + publisher + title, checkDigit)
        {
            Group = group;
            Publisher = publisher;
            Title = title;
            CheckDigit = checkDigit;
        }

        public RegistrationGroup Group { get; protected set; }
        public string Publisher { get; protected set; }
        public string Title { get; protected set; }

        public override string ToString(string seperator, bool forceValidCheckDigit)
        {
            return string.Concat(new object[]
            {
                Group.UccPrefix,
                seperator,
                Group.GroupIdentifier,
                seperator,
                Publisher,
                seperator,
                Title,
                seperator,
                forceValidCheckDigit ? CalculatedCheckDigit : CheckDigit
            });
        }
    }
}