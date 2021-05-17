using DocParty.Models;
using DocParty.Services.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.ViewModel
{
    public class ProjectSnapshotsTableData
    {
        public ReferencedTable Table { set; get; }
        public IEnumerable<Comment> [] Comments { set; get; }
    }
}
