using DocParty.Services.Tables;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DocParty.Services.Tables
{
    public class ObjectTable<T> : ITable where T : class
    {
        private readonly string[,] _values;
        private readonly string[] _columnNames;
        public string[] ColumnNames => _columnNames;
        public string[,] Values => _values;

        public ObjectTable(IEnumerable<T> objects)
        {
            _columnNames = typeof(T)
                .GetProperties()
                .Select(property => property.Name)
                .ToArray();
            _values = FillTable(objects);
        }

        private string[,] FillTable(IEnumerable<T> objects)
        {
            int rowCount = objects.Count();
            int columnCount = _columnNames.Length;

            var values = new string[rowCount,columnCount];

            for (int i = 0; i < rowCount; i++)
            {
                T item = objects.ElementAt(i);

                for (int j = 0; j < columnCount; j++)
                    values[i,j] = typeof(T)
                        .GetProperty(_columnNames[j])
                        .GetValue(item)
                        .ToString();
            }

            return values;
        }  
    }
}
