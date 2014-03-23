using System;
using System.IO;
using System.Xml.Linq;

namespace IsbnTools
{
    public class EmbeddedRangeMessageProvider : IRangeMessageProvider
    {
        public EmbeddedRangeMessageProvider(string resourceName = "IsbnTools.range.xml")
        {
            ResourceName = resourceName;
        }

        public string ResourceName { get; private set; }

        public virtual RangeMessage GetRangeMessage()
        {
            using (Stream stream = GetType().Assembly.GetManifestResourceStream(ResourceName))
            {
                return new RangeMessage(XDocument.Load(stream));
            }
        }
    }
}