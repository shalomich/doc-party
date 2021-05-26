using DocParty.Services.Tables;
using DocParty.Tests.TableTests;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DocParty.Tests
{
    
    public class ObjectTableTests
    {
        [Theory]
        [MemberData(nameof(GetCorrectTableCreateData))]
        public void CorrectTableCreate(IEnumerable<Human> humen)
        {
            int humanPropertyCount = typeof(Human).GetProperties().Count();

            ITable table = new ObjectTable<Human>(humen);

            Assert.Equal(humanPropertyCount, table.ColumnNames.Length);
            Assert.Equal(humen.Count(), table.RowCount);
        }

        public static IEnumerable<object[]> GetCorrectTableCreateData()
        {
            var address = new Address();
            var human = Enumerable.Repeat(new Human { Address = address }, 10);
            yield return new object[]
            {
                human.Take(3)
            };
            yield return new object[]
            {
                human.Take(0)
            };
            yield return new object[]
            {
                human
            };
        }
    }
}
