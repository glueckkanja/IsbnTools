using System;
using System.Xml.Linq;

namespace IsbnTools
{
    public class Range
    {
        private Range(int start, int end, int length)
        {
            Start = start;
            End = end;
            Length = length;
        }

        public int Start { get; private set; }
        public int End { get; private set; }
        public int Length { get; private set; }

        public static Range FromXml(XElement ruleElement)
        {
            var range = (string) ruleElement.Element("Range");

            string[] parts = range.Split('-');

            int start = int.Parse(parts[0]);
            int end = int.Parse(parts[1]);

            return new Range(start, end, (int) ruleElement.Element("Length"));
        }
    }
}