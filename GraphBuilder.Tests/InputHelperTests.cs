using Xunit;

namespace GraphBuilder.Tests
{
    public class InputHelperTests
    {
        [Fact]
        public void Parse_AcceptsCommaAndDot()
        {
            double value;

            Assert.True(InputHelper.TryParseNumber("2,5", out value));
            Assert.Equal(2.5, value);
            Assert.True(InputHelper.TryParseNumber("2.5", out value));
            Assert.Equal(2.5, value);
        }

        [Fact]
        public void Parse_RejectsWrongText()
        {
            double value;

            Assert.False(InputHelper.TryParseNumber("", out value));
            Assert.False(InputHelper.TryParseNumber("abc", out value));
            Assert.False(InputHelper.TryParseNumber("1e999", out value));
        }

        [Fact]
        public void CheckRange_FindsWrongValues()
        {
            Assert.Null(InputHelper.CheckRange(-10, 10, 0.1, -10, 10));

            Assert.NotNull(InputHelper.CheckRange(10, -10, 0.1, -10, 10));
            Assert.NotNull(InputHelper.CheckRange(-10, 10, 0.1, 5, 5));
            Assert.NotNull(InputHelper.CheckRange(-10, 10, 0, -10, 10));
            Assert.NotNull(InputHelper.CheckRange(-10, 10, 0.00001, -10, 10));
        }
    }
}
