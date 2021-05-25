using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.Models
{
    public class UnauthorizedAuthor
    {
        public int Id { set; get; }
        public string Email { set; get; }

        public Project Project { set; get; }
        public int ProjectId { set; get; }
    }
}
