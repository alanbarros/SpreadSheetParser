using System.Collections.Generic;
using SpreadSheetParser.Models;

namespace SpreadSheetParser.Extensions
{
    public static class SheetParserExtension
    {
        public static List<TContent> Parse<TContent>(this SheetFile<TContent> file)
            where TContent : SheetObject
        {
            return new SheetBuilder(file.File, file.SheetName).Build().TryParseList<TContent>();
        }
    }
}