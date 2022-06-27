using System;

namespace SpreadSheetParser.Models
{
    public class RowColumn
    {
        public RowColumn(object value, Type columnType, int index)
        {
            this.Index = index;
            this.Value = value;
            this.ColumnType = columnType;

        }

        public int Index { get; }
        public object Value { get; }
        public Type ColumnType { get; set; }
    }
}