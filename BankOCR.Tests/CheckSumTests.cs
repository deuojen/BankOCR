using BankOCR.Classes;
using BankOCR.Utils;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankOCR.Tests
{
    public class CheckSumTests
    {
        [Theory]
        [InlineData("711111111", true)]
        [InlineData("123456789", true)]
        [InlineData("490867715", true)]
        [InlineData("888888888", false)]
        [InlineData("490067715", false)]
        [InlineData("012345678", false)]
        [InlineData("0123456?8", false)]
        [InlineData(" ", false)]
        public void When_FlatImageFormatGiven_Return_Number(string input, bool expected)
        {
            bool actual = new CheckSum().Check(input);

            actual.Should().Be(expected);
        }
    }
}
