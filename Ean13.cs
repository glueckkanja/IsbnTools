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
        }

        public string Digits { get; protected set; }
        public int CheckDigit { get; protected set; }

        public bool HasValidCheckDigit
        {
            get { return CheckDigit == CalculateCheckDigit(); }
        }

        public static Ean13 Parse(string input, RangeMessage rangeMessage)
        {
            if (input == null)
            {
                return null;
            }

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
                    return new Isbn13Parser(rangeMessage).Parse(clean);
                }

                if (clean.Length == 13)
                {
                    return new Ean13(clean.Substring(0, 12), int.Parse(clean.Substring(12, 1)));
                }

                return new Ean13(clean.Substring(0, 12));
            }

            return null;
        }

        public override string ToString()
        {
            return ToString("-");
        }

        protected virtual string ToString(string seperator)
        {
            return Digits.Insert(3, seperator) + seperator + CheckDigit;
        }

        private int CalculateCheckDigit()
        {
            return CalculateEan13CheckDigit(Digits);
        }

        protected static int CalculateEan13CheckDigit(string digits)
        {
            int num = digits.Select((t, i) => (t - 48)*(i%2 == 0 ? 1 : 3)).Sum();

            return (10 - num%10)%10;
        }
    }
}