using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.Models
{
    class User : IdentityUser<int>, IEntity
    {
        public override int Id { set; get; }

        public IEnumerable<Project> Projects { set; get; }
        public IEnumerable<UserProjectRole> ProjectRoles { set; get; }

    }
}
