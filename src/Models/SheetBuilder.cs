using System;
using System.IO;
using ExcelDataReader;

namespace SpreadSheetParser.Models
{
    public class SheetBuilder
    {
        public string BookName { get; }
        public string SheetName { get; }
        private Sheet? sheet;

        public SheetBuilder(string bookName, string sheetName)
        {
            this.SheetName = sheetName;
            this.BookName = bookName;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }

        public Sheet Build()
        {
            using var stream = File.Open(BookName, FileMode.Open, FileAccess.Read);

            using var reader = ExcelReaderFactory.CreateReader(stream);

            SetSheet(reader);

            if (sheet is null)
                throw new NullReferenceException("The sheet cannot be null");

            int rowIndex = 0;

            while (reader.Read()) //Each ROW
            {
                if (rowIndex == 0)
                    SetHeader(reader, sheet);
                else
                    sheet.ContentRows.Add(ReadRow(rowIndex, sheet, reader));

                rowIndex++;
            }

            return sheet;
        }

        private void SetSheet(IExcelDataReader reader)
        {
            do
            {
                if (reader.Name == SheetName)
                {
                    this.sheet = new Sheet(reader.Name);
                    return;
                }

            } while (reader.NextResult());

            throw new ArgumentException($"Could not find a sheet called {SheetName}");
        }

        public void SetHeader(IExcelDataReader reader, Sheet sheet)
        {
            sheet.Header = new SheetHeader(ReadRow(0, sheet, reader));
        }

        private SheetRow ReadRow(int index, Sheet sheet, IExcelDataReader reader)
        {
            var row = new SheetRow(sheet, index);

            for (int column = 0; column < reader.FieldCount; column++)
            {
                Type columnType = reader.GetFieldType(column);
                object objectValue = reader.GetValue(column);

                row.AddColumn(new RowColumn(objectValue, columnType, column));
            }

            return row;
        }
    }
}