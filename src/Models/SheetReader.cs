using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using ExcelDataReader;

namespace SpreadSheetParser.Models
{
    public class SheetReader
    {
        protected Sheet Sheet { get; private set; }
        protected string SheetName { get; private set; }

        public SheetReader(string sheetName)
        {
            this.SheetName = sheetName;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }

        public static List<TSheet> ReadStream<TSheet>(Stream stream) where TSheet : SheetObject
        {
            var sheetReader = new SheetReader(GetSheetName<TSheet>());

            using var reader = ExcelReaderFactory.CreateReader(stream);

            return sheetReader.Read(reader).TryParseList<TSheet>();
        }

        protected Sheet Read(IExcelDataReader reader)
        {
            SetSheet(reader);

            if (Sheet is null)
                throw new NullReferenceException("The sheet cannot be null");

            int rowIndex = 0;

            while (reader.Read()) //Each ROW
            {
                if (rowIndex == 0)
                    SetHeader(reader, Sheet);
                else
                    Sheet.ContentRows.Add(ReadRow(rowIndex, Sheet, reader));

                rowIndex++;
            }

            return Sheet;
        }

        protected void SetSheet(IExcelDataReader reader)
        {
            do
            {
                if (reader.Name == SheetName)
                {
                    Sheet = new Sheet(reader.Name);
                    return;
                }

            } while (reader.NextResult());

            throw new ArgumentException($"Could not find a sheet called {SheetName}");
        }

        protected void SetHeader(IExcelDataReader reader, Sheet sheet)
        {
            sheet.Header = new SheetHeader(ReadRow(0, sheet, reader));
        }

        protected SheetRow ReadRow(int index, Sheet sheet, IExcelDataReader reader)
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

        public static string GetSheetName<TSheet>() where TSheet : SheetObject
        => typeof(TSheet).GetCustomAttributes(false)
                .Where(w => w.GetType()
                .Name.Contains("DisplayName"))
                .FirstOrDefault() is DisplayNameAttribute displayName ?
                displayName.DisplayName : typeof(TSheet).Name;
    }
}