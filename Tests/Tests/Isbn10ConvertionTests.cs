using System;
using System.Collections.Generic;
using System.Linq;
using IsbnTools;
using Xunit;
using Xunit.Extensions;

namespace Tests
{
    public class Isbn10ConvertionTests : IUseFixture<RangeMessageFixture>
    {
        private RangeMessage _rm;

        public void SetFixture(RangeMessageFixture data)
        {
            _rm = data.RangeMessage;
        }

        [Theory]
        [InlineData("0330284983", "978-0-330-28498-1")]
        [InlineData("9783718622", "978-978-37186-2-3")]
        [InlineData("9992158107", "978-99921-58-10-4")]
        [InlineData("9971502100", "978-9971-5-0210-2")]
        [InlineData("9604250590", "978-960-425-059-2")]
        [InlineData("8090273416", "978-80-902734-1-2")]
        [InlineData("8535902775", "978-85-359-0277-8")]
        [InlineData("1843560283", "978-1-84356-028-9")]
        [InlineData("0684843285", "978-0-684-84328-5")]
        [InlineData("080442957X", "978-0-8044-2957-3")]
        [InlineData("0851310419", "978-0-85131-041-1")]
        [InlineData("0943396042", "978-0-943396-04-0")]
        [InlineData("097522980X", "978-0-9752298-0-4")]
        public void Sample1(string test, string expected)
        {
            string actual = Ean13.Parse(test, _rm).ToString();
            Assert.Equal(actual, expected);
        }
    }
}