
using DocParty.RequestHandlers.Profile;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.RequestHandlers
{
    public class UserQuery<T> : IRequest<T>
    {
        public string UserName { set; get; }
    }
}
