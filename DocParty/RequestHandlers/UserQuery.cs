﻿using DocParty.RequestHandlers.UserProfile;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.RequestHandlers
{
    public class UserQuery : IRequest<UserStatistics>
    {
        public string UserName { set; get; }
    }
}
