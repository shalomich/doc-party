using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.RequestHandlers.CommentProject
{
    public class CommentFormData
    {
        [Required]
        public string SnapshotName { set; get; }
        
        [Required]
        public string Text { set; get; }
    }
}
