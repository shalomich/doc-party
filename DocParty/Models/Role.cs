using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.Models
{
    public class Role : IdentityRole<int>
    {
        public enum Value
        {
            Creator,
            Author,
            Visitor
        }
        public IEnumerable<UserProjectRole> ProjectRoles { set; get; }
    }
}
