using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.ViewModel
{
    public class ProjectInfo
    {
        public string Name { set; get; }
        public bool IsActive { set; get; }
        public string StateLocation { set; get; }
        public string SnapshotAddingLocation { set; get; }
        public CommentAddingFormData FormData {set;get;}
        public ProjectSnapshotsTableData Data { set; get; }
    }
}
