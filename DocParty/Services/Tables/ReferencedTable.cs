using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.Services.Tables
{
    public class ReferencedTable : TableDecorator
    {
        public IReadOnlyDictionary<string,string[]> ColumnReferences { get; }
        public ReferencedTable(ITable table, Dictionary<string,string> columnReferenceTemplates) : base(table)
        {
            ColumnReferences = columnReferenceTemplates
                .Select((columnReferenceTemplate) =>
                    KeyValuePair.Create(columnReferenceTemplate.Key,
                        CreateReferences(columnReferenceTemplate.Key, columnReferenceTemplate.Value)))
                .ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        private string[] CreateReferences(string columnName, string referenceTemplate)
        {
            if (ColumnNames.Contains(columnName) == false)
                throw new ArgumentException();

            var columnIndex = ColumnNames
                .Select((columnName, index) => new { ColumnName = columnName, Index = index })
                .Where(item => item.ColumnName == columnName)
                .Select(item => item.Index)    
                .First();

            int rowCount = Values.Length / ColumnNames.Length;

            var references = new string[rowCount];

            for (int i = 0; i < rowCount; i++)
                references[i] = String.Format(referenceTemplate, Values[i, columnIndex]);

            return references;
            
        }

        public override string[] ColumnNames => _table.ColumnNames;

        public override string[,] Values => _table.Values;
    }
}
