using System;
using System.Linq;

namespace IsbnTools
{
    internal class Isbn13Parser
    {
        public Isbn13Parser(RangeMessage rangeMessage)
        {
            RangeMessage = rangeMessage;
        }

        public RangeMessage RangeMessage { get; private set; }

        public Isbn13 Parse(string isbn13)
        {
            if (isbn13 == null) throw new ArgumentNullException("isbn13");

            isbn13 = isbn13.Trim();

            if (isbn13.Length != 12 && isbn13.Length != 13)
            {
                throw new NotSupportedException("Only ISBN-13 is supported.");
            }

            return ParseImpl(isbn13);
        }

        private Isbn13 ParseImpl(string isbn)
        {
            RegistrationGroup group = RangeMessage.FindGroup(isbn);

            if (group == null)
            {
                throw new Exception("No publisher group found. Unable to format.");
            }

            int publisherIndex = group.UccPrefix.Length + group.GroupIdentifier.Length;

            string lookup = isbn;

            if (lookup.Length == 12)
            {
                lookup += "0";
            }

            int intRange = int.Parse(lookup.Substring(publisherIndex, Math.Min(13 - publisherIndex, 7)));

            Range range = group.Rules.Single(x => intRange >= x.Start && intRange <= x.End);

            string publisher = isbn.Substring(publisherIndex, range.Length);

            int titleIndex = publisherIndex + range.Length;
            int titleLength = 13 - titleIndex - 1;

            string title = isbn.Substring(titleIndex, titleLength);

            if (isbn.Length == 13)
            {
                int checkDigit = int.Parse(isbn.Substring(12, 1));
                return new Isbn13(group.UccPrefix, group.GroupIdentifier, publisher, title, checkDigit);
            }

            return new Isbn13(@group.UccPrefix, @group.GroupIdentifier, publisher, title);
        }
    }
}