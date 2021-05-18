using DocParty.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.RequestHandlers.ProjectHandlers
{
    public class ProjectHandlerData<Request,Responce> :  HandlerData<Request, Responce>
    {
        public Project Project { set; get; }
    }
}
