using System;
using IsbnTools;

namespace Tests
{
    public class RangeMessageFixture
    {
        public RangeMessageFixture()
        {
            var rmp = new EmbeddedRangeMessageProvider();
            RangeMessage = rmp.GetRangeMessage();
        }

        public RangeMessage RangeMessage { get; private set; }
    }
}