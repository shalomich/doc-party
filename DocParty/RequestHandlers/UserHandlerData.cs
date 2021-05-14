using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.RequestHandlers
{
    public class UserHandlerData<Request,Responce> : HandlerData<Request,Responce>
    {
        public UserRequest UserRequest { set; get; }
    }
}
