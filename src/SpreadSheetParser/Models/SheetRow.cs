using System.Collections.Generic;

namespace SpreadSheetParser.Models
{
    public class SheetRow
    {
        private List<RowColumn> columns = new List<RowColumn>();
        private Sheet sheet;
        public int Index { get; set; }

        public SheetRow(Sheet sheet, int index)
        {
            Index = index;
            this.sheet = sheet;
        }

        public void AddColumn(RowColumn column)
        {
            columns.Add(column);
        }

        public List<RowColumn> GetColumns() => columns;

        public Sheet GetSheet() => sheet;
    }
}