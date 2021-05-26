using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.Services.Tables
{
    /// <summary>
    /// Table that add column with row numbers.
    /// </summary>
    public class NumberedTable : TableDecorator
    {
        private const string NumberingColumnName = "Number";

        private readonly string[] _columnNames;
        private readonly string[,] _values;

        public NumberedTable(ITable table) : base(table)
        {
            int rowCount = table.RowCount;
            int columnCount = table.ColumnNames.Length + 1;

            var values = new string[rowCount, columnCount];

            for (int i = 0; i < rowCount; i++)
            {
                values[i, 0] = Convert.ToString(i + 1);

                int sourceIndex = i * table.ColumnNames.Length;
                int destinationIndex = i * columnCount + 1;

                Array.Copy(table.Values, sourceIndex, values, destinationIndex, table.ColumnNames.Length);
            }

            _values = values;

            _columnNames = new string[] { NumberingColumnName }
                .Union(table.ColumnNames)
                .ToArray();       
        }


        public override string[] ColumnNames => _columnNames;

        public override string[,] Values => _values;
    }
}
