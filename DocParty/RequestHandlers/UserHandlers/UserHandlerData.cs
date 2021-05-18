using DocParty.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.RequestHandlers.UserHandlers
{
    class UserHandlerData<Request,Responce> : HandlerData<Request,Responce>
    {
        public User User { set; get; }
    }
}
