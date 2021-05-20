using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.RequestHandlers.UserHandlers.AddAuthor
{
    public class AuthorAddingFormData
    {
        public string ProjectName { set; get; }
        public string Email { set; get; }
    }
}
