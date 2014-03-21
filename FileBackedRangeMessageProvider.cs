using System;
using System.IO;
using System.Xml.Linq;

namespace IsbnTools
{
    public class FileBackedRangeMessageProvider : IRangeMessageProvider
    {
        private readonly string _filename = Path.GetTempPath() + "isbn-range-message-buffered.xml";
        private readonly IRangeMessageProvider _rangeMessageProvider;

        private RangeMessage _buffer;
        private DateTime _lastRefresh = DateTime.MinValue;
        private XDocument _xml;

        public FileBackedRangeMessageProvider(IRangeMessageProvider rangeMessageProvider)
            : this(rangeMessageProvider, TimeSpan.FromHours(24))
        {
        }

        public FileBackedRangeMessageProvider(IRangeMessageProvider rangeMessageProvider, TimeSpan maxAge)
        {
            MaxAge = maxAge;
            _rangeMessageProvider = rangeMessageProvider;
        }

        public TimeSpan MaxAge { get; private set; }

        public RangeMessage GetRangeMessage()
        {
            if (DateTime.UtcNow - _lastRefresh > MaxAge)
            {
                Refresh();
            }

            return _buffer;
        }

        private void Refresh()
        {
            bool fromFile = false;

            try
            {
                if (File.Exists(_filename))
                {
                    var info = new FileInfo(_filename);

                    if (DateTime.UtcNow - info.LastWriteTimeUtc < MaxAge)
                    {
                        _xml = XDocument.Load(_filename);
                        _buffer = new RangeMessage(_xml);
                        fromFile = true;
                    }
                }
            }
            catch
            {
            }

            if (!fromFile)
            {
                _buffer = _rangeMessageProvider.GetRangeMessage();
                _xml = _buffer.RangeMessageXml;

                try
                {
                    _xml.Save(_filename);
                }
                catch
                {
                }
            }

            _lastRefresh = DateTime.UtcNow;
        }
    }
}