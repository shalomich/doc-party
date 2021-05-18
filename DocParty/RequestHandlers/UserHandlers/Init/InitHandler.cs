using DocParty.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocParty.RequestHandlers.UserHandlers.Init
{
    class InitHandler : IRequestHandler<HandlerData<UserRoute, User>, User>
    {
        private ApplicationContext Context { get; }

        public InitHandler(ApplicationContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<User> Handle(HandlerData<UserRoute, User> request, CancellationToken cancellationToken)
        {
            return await Context.Users
               .FirstAsync(user => user.UserName == request.Data.UserName);
        }
    }
}
