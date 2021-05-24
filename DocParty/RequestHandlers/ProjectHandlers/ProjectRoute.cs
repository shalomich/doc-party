using DocParty.RequestHandlers.UserHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.RequestHandlers.ProjectHandlers
{
    public class ProjectRoute : UserRoute
    {
        public string ProjectName { set; get; }
    }
}
