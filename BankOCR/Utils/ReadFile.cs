using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankOCR.Interfaces;

namespace BankOCR.Utils
{
    public class ReadFile : IReadFile
    {
        public string FilePath { get; set; }
        public List<string> FileContent { get; set; }

        public ReadFile(string filePath)
        {
            FilePath = filePath;
            FileContent = new List<string>();
        }

        public List<string> GetContent()
        {
            if (!File.Exists(FilePath))
            {
                return FileContent;
            }

            try
            {
                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.
                using (StreamReader sr = new StreamReader(FilePath))
                {
                    string line;
                    // Read and display lines from the file until the end of
                    // the file is reached.
                    while ((line = sr.ReadLine()) != null)
                    {
                        if(line.Length != 27)
                        {
                            var addingCount = 27 - line.Length;
                            for (int i = 0; i < addingCount; i++)
                            {
                                line += " ";
                            }
                        }
                        FileContent.Add(line);
                    }
                }
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            return FileContent;
        }
    }
}
