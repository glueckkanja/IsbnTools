using System;
using IsbnTools;
using Xunit;
using Xunit.Extensions;

namespace Tests
{
    public class Isbn13Tests : IUseFixture<RangeMessageFixture>
    {
        private RangeMessage _rm;

        public void SetFixture(RangeMessageFixture data)
        {
            _rm = data.RangeMessage;
        }

        [Theory]
        [InlineData("978-0-330-28498-4")]
        [InlineData("978-0-9532277-7-8")]
        [InlineData("978-600-119-125-1")]
        [InlineData("978-601-7151-13-3")]
        [InlineData("978-602-8328-22-7")]
        [InlineData("978-603-500-045-1")]
        [InlineData("978-606-8126-35-7")]
        [InlineData("978-607-455-035-1")]
        [InlineData("978-608-203-023-4")]
        [InlineData("978-612-45165-9-7")]
        [InlineData("978-614-404-018-8")]
        [InlineData("978-615-5014-99-4")]
        [InlineData("978-978-37186-2-3")]
        [InlineData("978-988-00-3827-3")]
        [InlineData("978-9928-4005-2-9")]
        [InlineData("978-9929-8016-4-6")]
        [InlineData("978-9930-9431-0-6")]
        [InlineData("978-9933-10-147-3")]
        [InlineData("978-9934-0-1596-0")]
        [InlineData("978-99937-1-056-1")]
        [InlineData("978-99965-2-047-1")]
        public void Sample1(string expected)
        {
            string actual = Ean13.Parse(expected.Replace("-", ""), _rm).ToString();
            Assert.Equal(actual, expected);
        }

        [Theory]
        [InlineData("978-9929-8016-4-6")]
        [InlineData("978-9983-955-74-3")]
        [InlineData("978-9983-9907-6-8")]
        public void RequirePaddingWhenLookingUpRange(string expected)
        {
            string actual = Ean13.Parse(expected.Replace("-", ""), _rm).ToString();
            Assert.Equal(actual, expected);
        }

        [Theory]
        [InlineData("979-10-90636-07-1")]
        public void CanParse979Books(string expected)
        {
            string actual = Ean13.Parse(expected.Replace("-", ""), _rm).ToString();
            Assert.Equal(actual, expected);
        }

        [Theory]
        [InlineData("979-0-700303-32-7")]
        public void CannotParse979Ismn(string expected)
        {
            Assert.Throws<Exception>(() => Ean13.Parse(expected.Replace("-", ""), _rm));
        }
    }
}