using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.RequestHandlers
{
    public class HandlerData<Request,Responce> : IRequest<Responce>
    {
        public Request Data { set; get; } 
    }
}
