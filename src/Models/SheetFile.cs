using System.IO;

namespace SpreadSheetParser.Models
{
    public class SheetFile<TContent> where TContent : SheetObject
    {
        public string BookName { get; }
        public string SheetName { get; }

        public SheetFile(FileInfo file, string sheetName = null)
        {
            this.BookName = file.Name;
            this.SheetName = sheetName ?? SheetReader.GetSheetName<TContent>();
            File = file;
        }

        public FileInfo File { get; }
    }
}