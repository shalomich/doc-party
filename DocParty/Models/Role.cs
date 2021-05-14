using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.Models
{
    class Role : IdentityRole<int>
    {
        public enum Value
        {
            Creator,
            Author
        }
        public IEnumerable<UserProjectRole> ProjectRoles { set; get; }
    }
}
