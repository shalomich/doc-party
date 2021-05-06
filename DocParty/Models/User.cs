using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.Models
{
    class User : IEntity
    {
        public int Id { set; get; }
        public IEnumerable<Project> Projects { set; get; }
    }
}
