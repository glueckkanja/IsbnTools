using System;
using System.Xml.Linq;

namespace IsbnTools
{
    public class WebRangeMessageProvider : IRangeMessageProvider
    {
        /// <summary>
        ///     Get your range.xml here and upload it somewhere: https://www.isbn-international.org/range_file_generation
        /// </summary>
        public WebRangeMessageProvider(string uri)
        {
            Uri = uri;
        }

        public string Uri { get; private set; }

        public virtual RangeMessage GetRangeMessage()
        {
            return new RangeMessage(XDocument.Load(Uri));
        }
    }
}