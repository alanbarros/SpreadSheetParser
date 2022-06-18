using System.ComponentModel;
using SpreadSheetParser.Models;

namespace Tests
{

    public class SampleSheet : SheetObject
    {
        [DisplayName("First Column")]
        public Double FirstColumn { get; set; }

        [DisplayName("Second Column")]
        public Double SecondColumn { get; set; }

        public SampleSheet(SheetHeader header, SheetRow row) : base(header, row)
        {
            if (TryBuildObject<SampleSheet>(this) is false)
                throw new ArgumentException("Could not create a sample object");
        }

    }
}