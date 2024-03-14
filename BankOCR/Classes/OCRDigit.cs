using BankOCR.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankOCR.Classes
{
    /// <summary>
    /// Single digit data
    /// </summary>
    public class OCRDigit
    {
        public string FlatDigit { get; set; }
        public string FlatNumber { get; set; }
        public string CurrentNumber { get; set; }
        public List<int> PossibleNumbers { get; set; }

        public OCRDigit(string flatDigit)
        {
            FlatDigit = flatDigit;
            FlatNumber = Data.FlatImageToFlatNumber(FlatDigit);
            CurrentNumber = Data.GetOCRMatch(FlatNumber);
            PossibleNumbers = Data.GetPossibleOCRMatch(FlatNumber);
        }

        public override string ToString()
        {
            return $"{CurrentNumber}";
        }
    }

    /// <summary>
    /// Digit collection data
    /// </summary>
    public class OCRDigitCollection
    {
        public string OriginalInput { get; set; }

        public List<OCRDigit> OCRDigits { get; set; }

        public string CurrentNumbers { get; set; }

        public List<string> ValidPossibleCurrentNumbers { get; set; }

        public bool IsCheckSumRun { get; set; }

        public bool IsValid { get; set; }

        public OCRDigitCollection(string originalInput) { 
            OriginalInput = originalInput;
            OCRDigits = new List<OCRDigit>();
            CurrentNumbers = string.Empty;
            ValidPossibleCurrentNumbers = new List<string>();
            IsCheckSumRun = false;
            IsValid = false;
        }

        public void FillValidPossibleCurrentNumbers()
        {
            List<string> numberList = new List<string>();

            var newDigitCollection = "";

            for (int j = 0; j < 9; j++)
            {
                if (OCRDigits[j].PossibleNumbers.Count > 0)
                {
                    var arr = CurrentNumbers.ToCharArray().Select(c => c.ToString()).ToArray();

                    foreach (var number in OCRDigits[j].PossibleNumbers)
                    {
                        arr[j] = number.ToString();

                        newDigitCollection = string.Join("", arr);

                        var checkIfNewCollectionValid = new CheckSum().Check(newDigitCollection);
                        if (checkIfNewCollectionValid)
                        {
                            numberList.Add(newDigitCollection);
                        }
                    }
                }
            }


            // no need all multiple combinations as only one pipe/underscore missing or needs removed
            //double maxCombinations = Math.Pow(2, 9);

            //for (int i = 1; i < maxCombinations; i++)
            //{
            //    string binary = int.Parse(Convert.ToString(i, 2)).ToString("000000000");
            //    var arrIndex = binary.ToCharArray().Select(c => int.Parse(c.ToString())).ToArray();

            //    var newDigitCollection = "";

            //    for (int j = 0; j < 9; j++)
            //    {
            //        if (OCRDigits[j].PossibleNumbers.Count > 0 && OCRDigits[j].PossibleNumbers.Count <= arrIndex[j])
            //        {
            //            newDigitCollection += OCRDigits[j].PossibleNumbers[arrIndex[j] - 1].ToString();
            //        }
            //        else
            //        {
            //            newDigitCollection += OCRDigits[j].CurrentNumber;
            //        }                    
            //    }

            //    var checkIfNewCollectionValid = new CheckSum().Check(newDigitCollection);
            //    if (checkIfNewCollectionValid)
            //    {
            //        numberList.Add(newDigitCollection);
            //    }
            //}

            if(numberList.Count == 1) {
                CurrentNumbers = numberList[0];
                IsValid = true;
            }
            else if(numberList.Count > 0)
            {
                ValidPossibleCurrentNumbers = numberList;
                IsValid = true;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(OriginalInput);
            sb.Append(" \r\n\r\n=> ");
            if (ValidPossibleCurrentNumbers.Count > 0)
            {
                if (IsValid)
                {
                    sb.Append(CurrentNumbers + " AMB ['");
                }

                for (int i = 0; i < ValidPossibleCurrentNumbers.Count; i++)
                {
                    if (i == 0 && !IsValid)
                    {
                        sb.Append(ValidPossibleCurrentNumbers[i] + " AMB ['");
                    }
                    else if (i < ValidPossibleCurrentNumbers.Count - 1)
                    {
                        sb.Append(ValidPossibleCurrentNumbers[i] + "', ");
                    }
                    else
                    {
                        sb.Append(ValidPossibleCurrentNumbers[i] + "']");
                    }
                }
            }
            else
            {
                sb.Append(CurrentNumbers);
                if (CurrentNumbers.Contains("?"))
                {
                    sb.Append(" ILL");
                }
                else if (!IsValid && IsCheckSumRun)
                {
                    sb.Append(" ERR");
                }
            }

            return sb.ToString();
                
                //$"{OriginalInput} \r\n\r\n=> {CurrentNumbers}" 
                //+ (CurrentNumbers.Contains("?") ? " ILL" : !IsValid && IsCheckSumRun ? " ERR" : "")
                //+ (ValidPossibleCurrentNumbers.Count > 0 ? $" AMB {ValidPossibleCurrentNumbers}" : "");
        }
    }
}
