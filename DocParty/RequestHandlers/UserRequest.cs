
using DocParty.RequestHandlers.Profile;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.RequestHandlers
{
    public class UserRequest
    {
        public string UserName { set; get; }
    }
}
