using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.Models
{
    class UserProjectRole : IdentityUserRole<int>
    {
        public Project Project { set; get; }
        public virtual User User { set; get; }
        public virtual Role Role { set; get; }
    }
}
