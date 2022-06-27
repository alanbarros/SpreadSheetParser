using System;
using System.Linq;

namespace SpreadSheetParser.Models
{
    public class SheetHeader
    {
        public SheetRow Header { get; }

        public SheetHeader(SheetRow header)
        {
            if (header.GetColumns().Any(column => column.ColumnType != "".GetType()))
                throw new Exception("The Header Row should contains only string types");

            Header = header;
        }
    }
}