using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.Models
{
    class ProjectUserRole : IdentityUserRole<int>
    {
        public Project Project { set; get; }
    }
}
