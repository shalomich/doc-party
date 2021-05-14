using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.RequestHandlers
{
    public class ProjectRequest : UserRequest
    {
        public string ProjectName { set; get; }
    }
}
