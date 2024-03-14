using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankOCR.Interfaces;

namespace BankOCR.Utils
{
    public class CheckSum : ICheckSum
    {
        public CheckSum() { }

        public bool Check(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            { 
                return false;
            };

            if (input.Contains("?"))
            {
                return false;
            };

            var arr = input.ToCharArray().Select(c => c.ToString()).ToArray();
            var currentNumber = 0;

            for (int i = 1; i <= arr.Length; i++)
            {
                currentNumber += i * int.Parse(arr[9 - i]);
            }

            return currentNumber % 11 == 0 ? true : false;
        }
    }
}
