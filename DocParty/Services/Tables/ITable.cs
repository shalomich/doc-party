using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.Services.Tables
{
    public interface ITable
    {
        public string[] ColumnNames { get; }        
        public string[,] Values { get; }
        public int RowCount => Values.Length / ColumnNames.Length;
    }
}
