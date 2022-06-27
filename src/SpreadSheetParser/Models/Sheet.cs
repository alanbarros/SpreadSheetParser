using System;
using System.Collections.Generic;

namespace SpreadSheetParser.Models
{
    public class Sheet
    {
        public Sheet(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }
        public SheetHeader? Header { get; set; }
        public List<SheetRow> ContentRows { get; set; } = new List<SheetRow>();

        public List<T> TryParseList<T>() where T : SheetObject
        {
            var objs = new List<T>();

            foreach (var row in this.ContentRows)
            {
                if (Activator.CreateInstance(typeof(T), this.Header, row) is T instance)
                    objs.Add(instance);
                else
                    throw new Exception($"Could not create the {typeof(T).Name} instance");
            }

            return objs;
        }
    }
}



