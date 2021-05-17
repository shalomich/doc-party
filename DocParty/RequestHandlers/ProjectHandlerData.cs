using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.RequestHandlers
{
    public class ProjectHandlerData<Request,Responce> :  HandlerData<Request, Responce>
    {
        public ProjectRequest ProjectRequest { set; get; }
    }
}
