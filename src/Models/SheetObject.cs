using System;
using System.ComponentModel;
using System.Linq;

namespace SpreadSheetParser.Models
{
    public abstract class SheetObject
    {
        protected readonly SheetRow row;
        protected readonly SheetHeader header;
        public SheetObject(SheetHeader header, SheetRow row)
        {
            this.header = header;
            this.row = row;
        }

        protected bool TryGetProperty<T>(string propertyName, out T property)
        {
            try
            {
                int propertyIndex = header.Header
                    .GetColumns()
                    .First(c => c.Value.ToString() == propertyName)
                    .Index;

                var column = row.GetColumns()[propertyIndex];

                if (column.ColumnType == typeof(T))
                    if ((T)column.Value is T instance)
                    {
                        property = instance;

                        return true;
                    }
            }
            catch (System.Exception)
            {
            }

            property = Activator.CreateInstance<T>();

            return false;
        }

        protected bool TryGetProperty(string propertyName, Type type, out object? property)
        {
            try
            {
                int propertyIndex = header.Header
                    .GetColumns()
                    .First(c => c.Value.ToString() == propertyName)
                    .Index;

                var column = row.GetColumns()[propertyIndex];

                if (column.ColumnType == type)
                {
                    property = column.Value;

                    return true;
                }
            }
            catch (System.Exception)
            {
            }

            property = null;

            return false;
        }

        protected bool TryBuildObject<T>(T obj) where T : SheetObject
        {
            try
            {
                var properties = obj.GetType().GetProperties();

                foreach (var p in properties)
                {
                    var name = p.GetCustomAttributes(false)
                        .Where(w => w.GetType()
                        .Name.Contains("DisplayName"))
                        .FirstOrDefault() is DisplayNameAttribute displayName ? displayName.DisplayName : p.Name;

                    var type = p.PropertyType;

                    object? value = Activator.CreateInstance(type);

                    if (TryGetProperty(name, type, out value))
                        p.SetValue(obj, value);
                }

                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }
    }
}

