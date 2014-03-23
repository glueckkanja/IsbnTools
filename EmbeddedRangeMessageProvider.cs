using System;
using System.IO;
using System.IO.Compression;
using System.Xml.Linq;

namespace IsbnTools
{
    public class EmbeddedRangeMessageProvider : IRangeMessageProvider
    {
        public EmbeddedRangeMessageProvider(string resourceName = "IsbnTools.range.gz")
        {
            ResourceName = resourceName;
        }

        public string ResourceName { get; private set; }

        public virtual RangeMessage GetRangeMessage()
        {
            using (Stream stream = GetType().Assembly.GetManifestResourceStream(ResourceName))
            {
                if (ResourceName.EndsWith(".gz"))
                {
                    using (var gz = new GZipStream(stream, CompressionMode.Decompress))
                    {
                        return new RangeMessage(XDocument.Load(gz));
                    }
                }

                return new RangeMessage(XDocument.Load(stream));
            }
        }
    }
}