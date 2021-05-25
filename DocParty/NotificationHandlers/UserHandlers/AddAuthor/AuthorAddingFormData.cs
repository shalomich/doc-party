using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.NotificationHandlers.UserHandlers.AddAuthor
{
    public class AuthorAddingFormData
    {
        [Required]
        public string ProjectName { set; get; }
        
        [Required]
        public string Email { set; get; }
    }
}
