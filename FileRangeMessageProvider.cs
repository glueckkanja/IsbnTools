using System;
using System.Xml.Linq;

namespace IsbnTools
{
    public class FileRangeMessageProvider : IRangeMessageProvider
    {
        /// <summary>
        ///     Get your range.xml here: https://www.isbn-international.org/range_file_generation
        /// </summary>
        public FileRangeMessageProvider(string filename)
        {
            Filename = filename;
        }

        public string Filename { get; private set; }

        public virtual RangeMessage GetRangeMessage()
        {
            return new RangeMessage(XDocument.Load(Filename));
        }
    }
}