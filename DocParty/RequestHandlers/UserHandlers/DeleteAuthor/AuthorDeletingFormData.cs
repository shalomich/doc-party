using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.RequestHandlers.UserHandlers.DeleteAuthor
{
    public class AuthorDeletingFormData
    {
        public string UserName { set; get; }
        public string ProjectName { set; get; }
    }
}
