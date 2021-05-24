using DocParty.RequestHandlers.ProjectHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.RequestHandlers.SnapshotHandlers
{
    public class SnapshotRoute : ProjectRoute
    {
        public string SnapshotName { set; get; }
    }
}
