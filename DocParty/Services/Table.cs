using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DocParty.Services
{
    public class Table<T> where T : class
    {
        private string[][] _values;
        private string[] _columnNames;
        public string[] ColumnNames => _columnNames;
        public void Fill(IEnumerable<T> data)
        {
            _columnNames = typeof(T).GetProperties().Select(property => property.Name).ToArray();

            _values = new string[data.Count()][];

            int i = 0;
            foreach (T item in data)
            {
                _values[i] = new string[_columnNames.Length];
                for (int j = 0; j < _columnNames.Length; j++)
                    _values[i][j] = typeof(T).GetProperty(_columnNames[i]).GetValue(item).ToString();
                i++;
            }
        }

        public IEnumerable<string[]> GetRows()
        {
            foreach (var row in _values)
                yield return row;
        }  
    }
}
