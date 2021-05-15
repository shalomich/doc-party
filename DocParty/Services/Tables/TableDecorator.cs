using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.Services.Tables
{
    public abstract class TableDecorator : ITable
    {
        protected ITable _table;
        protected TableDecorator(ITable table)
        {
            _table = table ?? throw new ArgumentNullException(nameof(table));
        }

        public abstract string[] ColumnNames { get; }
        public abstract string[,] Values { get; }
    }
}
