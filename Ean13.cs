using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace IsbnTools
{
    public class Ean13
    {
        private static readonly Regex CleanUpRegex = new Regex("[^0-9Xx]");

        private Ean13(string digits) : this(digits, CalculateEan13CheckDigit(digits))
        {
        }

        protected Ean13(string digits, int checkDigit)
        {
            if (digits == null) throw new ArgumentNullException("digits");

            digits = digits.Trim();

            if (digits.Any(c => c < '0' || c > '9'))
                throw new ArgumentOutOfRangeException("digits", "Only digits are allowed.");

            if (digits.Length < 12)
                throw new ArgumentException("EAN-13 is too short.", "digits");

            if (digits.Length == 13)
                throw new ArgumentException("EAN-13 must not include the check digit.", "digits");

            if (digits.Length > 13)
                throw new ArgumentException("EAN-13 is too long.", "digits");

            if (checkDigit < 0 || checkDigit > 9)
                throw new ArgumentOutOfRangeException("checkDigit", "EAN-13 check digits must be a single digit (0-9).");

            Digits = digits;
            CheckDigit = checkDigit;
            CalculatedCheckDigit = CalculateEan13CheckDigit(Digits);
        }

        public int CalculatedCheckDigit { get; protected set; }
        public int CheckDigit { get; protected set; }
        public string Digits { get; protected set; }

        public static Ean13 Parse(string input, RangeMessage rangeMessage)
        {
            if (input == null) return null;

            string clean = CleanUpRegex.Replace(input, "").ToUpperInvariant();

            if (clean.Length == 9 || clean.Length == 10)
            {
                return Parse("978" + clean.Substring(0, 9), rangeMessage);
            }

            if (clean.Length == 12 || clean.Length == 13)
            {
                string country = clean.Substring(0, 3);

                if (rangeMessage.FindUccPrefix(country) != null)
                {
                    Isbn13 isbn = ParseIsbn13(clean, rangeMessage);

                    if (isbn != null)
                        return isbn;
                }

                if (clean.Length == 13)
                {
                    return new Ean13(clean.Substring(0, 12), clean[12] - 48);
                }

                return new Ean13(clean.Substring(0, 12));
            }

            return null;
        }

        protected static int CalculateEan13CheckDigit(string digits)
        {
            int num = digits.Select((t, i) => (t - 48)*(i%2 == 0 ? 1 : 3)).Sum();

            return (10 - num%10)%10;
        }

        public override string ToString()
        {
            return ToString("-", false);
        }

        public virtual string ToString(string seperator, bool forceValidCheckDigit)
        {
            return Digits.Insert(3, seperator) + seperator + (forceValidCheckDigit ? CalculatedCheckDigit : CheckDigit);
        }

        private static Isbn13 ParseIsbn13(string isbn, RangeMessage rangeMessage)
        {
            if (isbn == null) return null;

            isbn = isbn.Trim();

            RegistrationGroup group = rangeMessage.FindGroup(isbn);

            if (group == null)
            {
                // No publisher group found. Unable to format.
                return null;
            }

            int publisherIndex = group.UccPrefix.Prefix.Length + group.GroupIdentifier.Length;

            // rules in the range message are always padded to 7 digits
            // we can safely use the check digit too
            string paddedIsbn = isbn + "0000000000000";
            int paddedRange = int.Parse(paddedIsbn.Substring(publisherIndex, 7));

            Range range = group.Rules.Single(x => paddedRange >= x.Start && paddedRange <= x.End);

            if (range.Length == 0)
            {
                // Range not defined for use.
                return null;
            }

            string publisher = isbn.Substring(publisherIndex, range.Length);

            int titleIndex = publisherIndex + range.Length;
            int titleLength = 13 - titleIndex - 1;

            string title = isbn.Substring(titleIndex, titleLength);

            if (isbn.Length == 13)
            {
                int checkDigit = int.Parse(isbn.Substring(12, 1));
                return new Isbn13(group, publisher, title, checkDigit);
            }

            return new Isbn13(group, publisher, title);
        }
    }
}