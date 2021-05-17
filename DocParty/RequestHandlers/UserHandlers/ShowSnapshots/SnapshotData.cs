using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.RequestHandlers.ShowSnapshots
{
    public class SnapshotData
    {
        public string SnapshotName { set; get; }
        public string ProjectName { set; get; }
        public int CommentCount { set; get; }
        public string Description { set; get; }
    }
}
