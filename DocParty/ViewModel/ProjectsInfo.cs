using DocParty.RequestHandlers.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.ViewModel
{
    public class ProjectsInfo
    {
        public IEnumerable<ProjectData> Data { set; get; } 
        public string UserName { set; get; }
    }
}
