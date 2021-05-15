using DocParty.RequestHandlers.Projects;
using DocParty.Services.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.ViewModel
{
    public class ProjectsInfo
    {
        public ReferencedTable Table { set; get; } 
        public string UserName { set; get; }
    }
}
