using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.RequestHandlers.Projects
{
    public class ProjectTableData
    {
        public string ProjectName { set; get; }
        public string CreatorName { set; get; }
        public int AuthorCount { set; get; }
        public int SnapshotCount { set; get; }
        public bool IsActive { set; get; }
    }
}
