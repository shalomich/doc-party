using DocParty.RequestHandlers.Projects;
using DocParty.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.ViewModel
{
    public class ProjectDataTableInfo
    {
        public Table<ProjectData> Table { set; get; }
        public Dictionary<string, string> ProjectsLocations { set; get; }
    }
}
