namespace BankOCR.Interfaces
{
    public interface IReadFile
    {
        List<string> FileContent { get; set; }
        string FilePath { get; set; }

        List<string> GetContent();
    }
}