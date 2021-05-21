using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.ViewModel
{
    public class CommentAddingFormData
    {
        public string CommentLocation { set; get; }
        public SelectList SnapshotChoice { set; get; }
    }
}
