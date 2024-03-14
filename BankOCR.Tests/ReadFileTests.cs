using BankOCR.Utils;
using FluentAssertions;
using System.Text;

namespace BankOCR.Tests
{
    public class ReadFileTests
    {
        [Fact]
        public void When_File_Path_Not_Exists_Return_Empty_Content()
        {
            ReadFile parseFile = new ReadFile("randomfile.txt");
            var fileContent = parseFile.GetContent();

            fileContent.Should().BeEmpty();
        }

        [Fact]

        public void When_File_Exists_Return_Content()
        {
            string path = @".\temp\MyTest.txt";
            string directory = @".\temp";

            if (File.Exists(path))
            {
                File.Delete(path);
            }
            if(!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string content = "This is some text in the file.";

            try
            {
                // Create the file, or overwrite if the file exists.
                using (FileStream fs = File.Create(path))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(content);
                    // Add some information to the file.
                    fs.Write(info, 0, info.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            ReadFile parseFile = new ReadFile(path);
            var fileContent = parseFile.GetContent();

            fileContent.First().Should().Be(content);
        }
    }
}