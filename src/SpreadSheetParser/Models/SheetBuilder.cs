using System.IO;
using ExcelDataReader;

namespace SpreadSheetParser.Models
{
    public class SheetBuilder : SheetReader
    {
        public FileInfo FileInfo { get; set; }

        public SheetBuilder(FileInfo fileInfo, string sheetName) : base(sheetName)
        {
            this.FileInfo = fileInfo;
        }

        public Sheet Build()
        {
            using var stream = File.Open(FileInfo.FullName, FileMode.Open, FileAccess.Read);

            using var reader = ExcelReaderFactory.CreateReader(stream);

            return Read(reader);
        }
    }
}