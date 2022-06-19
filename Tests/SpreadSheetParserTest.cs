using SpreadSheetParser.Models;
using Xunit;
using FluentAssertions;

namespace Tests
{
    public class SpreadSheetParserTest
    {
        [Fact]
        public void ShouldParseSpreadSheet()
        {
            // Given
            var file = new FileInfo("Book.xlsx");
            var builder = new SheetBuilder(file, "First Sheet");

            // When
            Sheet book = builder.Build();
            List<SampleSheet> sampleObjects = book.TryParseList<SampleSheet>();

            // Then
            sampleObjects.Count.Should().Be(40);
        }
    }
}