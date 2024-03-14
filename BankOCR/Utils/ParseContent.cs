using BankOCR.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankOCR.Utils
{
    public class ParseContent
    {
        /// <summary>
        /// Convert Lines to readable digits
        /// </summary>
        /// <param name="fileContent"></param>
        /// <returns></returns>
        public List<OCRDigitCollection> GetOCRDigitCollection(List<string> fileContent)
        {
            var result = new List<OCRDigitCollection>();

            var index = 0;
            var originalLine = "";
            var part1 = "";
            var part2 = "";
            var part3 = "";
            var part4 = "";
            var part5 = "";
            var part6 = "";
            var part7 = "";
            var part8 = "";
            var part9 = "";


            foreach (var line in fileContent)
            {
                if (index == 3)
                {
                    var digitCollection = new OCRDigitCollection(originalLine)
                    {
                        OCRDigits = new List<OCRDigit>()
                        {
                            new OCRDigit(part1),
                            new OCRDigit(part2),
                            new OCRDigit(part3),
                            new OCRDigit(part4),
                            new OCRDigit(part5),
                            new OCRDigit(part6),
                            new OCRDigit(part7),
                            new OCRDigit(part8),
                            new OCRDigit(part9),
                        }
                    };

                    result.Add(digitCollection);
                    originalLine = "";
                    part1 = "";
                    part2 = "";
                    part3 = "";
                    part4 = "";
                    part5 = "";
                    part6 = "";
                    part7 = "";
                    part8 = "";
                    part9 = "";
                    index = 0;
                    continue;
                }

                originalLine += (index == 0 ? "" : "\r\n") + line;
                part1 += (index == 0 ? "" : "\r\n") + line.Substring(0, 3);
                part2 += (index == 0 ? "" : "\r\n") + line.Substring(3, 3);
                part3 += (index == 0 ? "" : "\r\n") + line.Substring(6, 3);
                part4 += (index == 0 ? "" : "\r\n") + line.Substring(9, 3);
                part5 += (index == 0 ? "" : "\r\n") + line.Substring(12, 3);
                part6 += (index == 0 ? "" : "\r\n") + line.Substring(15, 3);
                part7 += (index == 0 ? "" : "\r\n") + line.Substring(18, 3);
                part8 += (index == 0 ? "" : "\r\n") + line.Substring(21, 3);
                part9 += (index == 0 ? "" : "\r\n") + line.Substring(24, 3);
                index++;
            }

            if (!String.IsNullOrWhiteSpace(originalLine))
            {
                var digitCollection = new OCRDigitCollection(originalLine)
                {
                    OCRDigits = new List<OCRDigit>()
                        {
                            new OCRDigit(part1),
                            new OCRDigit(part2),
                            new OCRDigit(part3),
                            new OCRDigit(part4),
                            new OCRDigit(part5),
                            new OCRDigit(part6),
                            new OCRDigit(part7),
                            new OCRDigit(part8),
                            new OCRDigit(part9),
                        }
                };

                result.Add(digitCollection);
            }

            return result;
        }

        /// <summary>
        /// Process flat digits to numbers
        /// </summary>
        /// <param name="digitCollections"></param>
        public void ProcessOCRDigitCollection(List<OCRDigitCollection> digitCollections)
        {
            foreach (var digitCollection in digitCollections)
            {
                var currentNumber = "";
                foreach (var digit in digitCollection.OCRDigits)
                {
                    Data.OCRDataMap.TryGetValue(digit.FlatNumber, out string? value);
                    if(!string.IsNullOrEmpty(value))
                    {
                        currentNumber += value?.ToString();
                    }
                    else
                    {
                        currentNumber += "?";
                    }
                }

                digitCollection.CurrentNumbers = currentNumber;
            }
        }

        /// <summary>
        /// Run checksum to check digits if valid
        /// </summary>
        /// <param name="digitCollections"></param>
        public void CheckDigits(List<OCRDigitCollection> digitCollections)
        {
            foreach (var digitCollection in digitCollections)
            {
                digitCollection.IsCheckSumRun = true;
                digitCollection.IsValid = new CheckSum().Check(digitCollection.CurrentNumbers);
            }
        }

        /// <summary>
        /// Re process collection for possible outputs
        /// </summary>
        /// <param name="digitCollections"></param>
        public void ReProcessOCRDigitCollection(List<OCRDigitCollection> digitCollections)
        {
            foreach (var digitCollection in digitCollections)
            {
                digitCollection.FillValidPossibleCurrentNumbers();

                //digitCollection.IsCheckSumRun = false;
                
            }
        }
    }
}
