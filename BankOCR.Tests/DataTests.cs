using BankOCR.Classes;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankOCR.Tests
{
    public class DataTests
    {
        public static TheoryData<string, string> FlatImageTestData =>
           new TheoryData<string, string>
           {
                { Data.Number0, "0" },
                { Data.Number1, "1" },
                { Data.Number2, "2" },
                { Data.Number3, "3" },
                { Data.Number4, "4" },
                { Data.Number5, "5" },
           };

        [Theory]
        [MemberData(nameof(FlatImageTestData))]
        public void When_FlatImageFormatGiven_Return_Number(string input, string expected)
        {
            string converted = Data.FlatImageToFlatNumber(input);
            Data.OCRDataMap.TryGetValue(converted, out string? actual);

            actual.Should().Be(expected);
        }

        public static TheoryData<string, List<int>> PossiblityTestData =>
           new TheoryData<string, List<int>>
           {
                { @" _ 
| |
|_|", new List<int>() { 8 }  },
                { @" _ 
|_|
 _|", new List<int>() { 5, 8 }  },
                { @" _ 
| |
| |", new List<int>() { 0 }  },
                { @"   
|_|
 _|", new List<int>() { 9 }  },
           };

        [Theory]
        [MemberData(nameof(PossiblityTestData))]
        public void When_RandomFlatImageFormatGiven_Return_Number(string input, List<int> expected)
        {
            string converted = Data.FlatImageToFlatNumber(input.Replace("\n", "").Replace("\r", ""));
            List<int> actual = Data.GetPossibleOCRMatch(converted);

            actual.Should().Equal(expected);
        }

        [Fact]
        public void Test_Max_Possible_Numbers()
        {
            double maxCombinations = Math.Pow(2, 6);
            int maxCount = 0;
            // with no top underscore "   "
            for (int i = 0; i < maxCombinations; i++)
            {
                string binary = int.Parse(Convert.ToString(i, 2)).ToString("000000");
                List<int> actual = Data.GetPossibleOCRMatch("000" + binary);
                maxCount = actual.Count > maxCount ? actual.Count : maxCount;
            }

            int maxCount2 = 0;
            // with top underscore " _ "
            for (int i = 0; i < maxCombinations; i++)
            {
                string binary = int.Parse(Convert.ToString(i, 2)).ToString("000000");
                List<int> actual2 = Data.GetPossibleOCRMatch("010" + binary);

                maxCount2 = actual2.Count > maxCount2 ? actual2.Count : maxCount2;
            }


            int value = 8;
            string binary2 = Convert.ToString(value, 3);

            // binary to integer
            // int value = Convert.ToInt32("1101", 2);

            // max possibilty 3
            maxCount.Should().Be(2);

            maxCount2.Should().Be(3);
        }
    }
}
