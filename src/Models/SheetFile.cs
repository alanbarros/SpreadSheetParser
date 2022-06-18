using System.ComponentModel;
using System.IO;
using System.Linq;

namespace SpreadSheetParser.Models
{
    public class SheetFile<TContent> where TContent : SheetObject
    {
        public string BookName { get; }
        public string SheetName { get; }

        public SheetFile(FileInfo file, string sheetName = null)
        {
            this.BookName = file.Name;
            this.SheetName = sheetName ?? GetSheetName();
            File = file;
        }

        private string GetSheetName() => typeof(TContent).GetCustomAttributes(false)
                        .Where(w => w.GetType()
                        .Name.Contains("DisplayName"))
                        .FirstOrDefault() is DisplayNameAttribute displayName ? displayName.DisplayName : typeof(TContent).Name;

        public FileInfo File { get; }
    }
}