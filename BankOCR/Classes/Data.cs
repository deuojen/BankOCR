using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace BankOCR.Classes
{
    public class Data
    {

        /// <summary>
        /// _ 
        ///| |
        ///|_|
        /// </summary>
        public static string Number0 = " _ | ||_|";

        /// <summary>
        ///   
        ///  |
        ///  |
        /// </summary>
        public static string Number1 = "     |  |";

        /// <summary>
        /// _ 
        /// _|
        ///|_ 
        /// </summary>
        public static string Number2 = " _  _||_ ";

        /// <summary>
        /// _ 
        /// _|
        /// _|
        /// </summary>
        public static string Number3 = " _  _| _|";

        /// <summary>
        ///   
        ///|_|
        ///  |
        /// </summary>
        public static string Number4 = "   |_|  |";


        /// <summary>
        /// _ 
        ///|_ 
        /// _|
        /// </summary>
        public static string Number5 = " _ |_  _|";

        /// <summary>
        /// _ 
        ///|_ 
        ///|_|
        /// </summary>
        public static string Number6 = " _ |_ |_|";

        /// <summary>
        /// _ 
        ///  |
        ///  |
        /// </summary>
        public static string Number7 = " _   |  |";

        /// <summary>
        /// _ 
        ///|_|
        ///|_|
        /// </summary>
        public static string Number8 = " _ |_||_|";

        /// <summary>
        /// _ 
        ///|_|
        /// _|
        /// </summary>
        public static string Number9 = " _ |_| _|";

        /// <summary>
        /// Return simplified line for number input
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string FlatImageToFlatNumber(string input)
        {
            return input.Replace("\n", "").Replace("\r", "").Replace(' ', '0').Replace('|', '1').Replace('_', '1');
        }

        /// <summary>
        /// Get number representative from flat image
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetOCRMatch(string input)
        {
            string result = "?";

            OCRDataMap.TryGetValue(input, out string? value);
            if (value != null)
            {
                result = value.ToString();
            }

            return result;
        }

        /// <summary>
        /// Flat image data to number representative
        /// </summary>
        public static Dictionary<string, string> OCRDataMap { get; set; } = new Dictionary<string, string>()
        {
            {"010101111", "0" }, // " _ | ||_|" 
            {"000001001", "1" }, // "     |  |"
            {"010011110", "2" }, // " _  _||_ "
            {"010011011", "3" }, // " _  _| _|"
            {"000111001", "4" }, // "   |_|  |"
            {"010110011", "5" }, // " _ |_  _|"
            {"010110111", "6" }, // " _ |_ |_|"
            {"010001001", "7" }, // " _   |  |"
            {"010111111", "8" }, // " _ |_||_|"
            {"010111011", "9" }  // " _ |_| _|"
        };

        /// <summary>
        /// Get possible numbers representative from flat image
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static List<int> GetPossibleOCRMatch(string input)
        {
            List<int> result = new List<int>();

            for (int i = 0; i < input.Length; i++)
            {
                // ignore first and third char as never changes
                if (i == 0 || i == 2)
                {
                    continue;
                }

                var arr = input.ToCharArray().Select(c => c.ToString()).ToArray();
                // swicth one bit to test
                arr[i] = arr[i] == "0" ? "1" : "0";

                var possibleNumber = GetOCRMatch(string.Join("", arr));
                if (possibleNumber != "?")
                {
                    result.Add(int.Parse(possibleNumber));
                }
            }

            return result;
        }

        /// <summary>
        /// Hard coded possiblity mapping rather than calculate each time
        /// </summary>
        [Obsolete("For now we can use switch bit and search in OCRDataMap will work better for find possiblities")]
        public static Dictionary<string, List<int>> OCRDataPossiblityMap { get; set; } = new Dictionary<string, List<int>>()
        {
            {"010101111", new List<int> () { 8 } }, // 0 -> 8 "010111111"
            {"000001001", new List<int> () { 7 } }, // 1 -> 7 "010001001"
            {"010011011", new List<int> () { 9 } }, // 3 -> 9 "010001001"
            {"010110011", new List<int> () { 6, 9 } }, // 5 -> 6, 9 "010110111", "010111011"
            {"010110111", new List<int> () { 5, 8 } }, // 6 -> 5, 8 "010110011", "010111111"
            {"010001001", new List<int> () { 1 } }, // 7 -> 1 "000001001"
            {"010111111", new List<int> () { 0, 6, 9 } }, // 8 -> 0, 6, 9 "010101111", "010110111", "010111011"
            {"010111011", new List<int> () { 5, 8 } }, // 9 -> 5, 8 "010110011", "010111111"
            // odd cases where does not represent any number until remove/add pipe or underscore
            {"000011001", new List<int> () { 1, 4 } }, // -1 -> 1, 4 "000001001", "000111001"
            {"010011001", new List<int> () { 3, 7 } }, // reverse F -> 3, 7 "010011011", "010001001"
            {"000011011", new List<int> () { 3 } }, // reverse upside F -> 3 "010011011"
            {"010111001", new List<int> () { 4, 9 } }, // q -> 4, 9 "000111001", "010111011"
            {"010110010", new List<int> () { 5 } }, // c underscore -> 5 "010110011"
            {"010010011", new List<int> () { 3, 5 } }, // underscore reverse c -> 3, 5 "010011011", "010110011"
            {"010011010", new List<int> () { 2, 3 } }, // reverse c underscore -> 2, 3 "010011110", "010011011"
            {"010111110", new List<int> () { 2, 3, 8 } }, // reverse 9 -> 2, 3, 8 "010011110", "010011110", "010111111"
            {"000101111", new List<int> () { 0 } }, // U -> 0 "010101111"
            {"010101011", new List<int> () { 0, 9 } }, // up pipe reverse C -> 0, 9 "010101111", "010111011"
            {"010001111", new List<int> () { 0 } }, // down pipe reverse C -> 0 "010101111"
            {"010101101", new List<int> () { 0 } }, // upside U -> 0 "010101111"
            {"010101110", new List<int> () { 0 } }, // up pipe C -> 0 "010101111"
            {"010100111", new List<int> () { 0, 6 } }, // down pipe C -> 0, 6 "010101111", "010110111"
            {"000111011", new List<int> () { 9 } }, // Y underscore -> 9 "010111011"
            {"010111101", new List<int> () { 8 } }, // A -> 8 "010111111"
        };
    }
}
