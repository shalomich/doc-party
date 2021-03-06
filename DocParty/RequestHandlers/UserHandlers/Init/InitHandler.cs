using DocParty.Exceptions;
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

        /// <summary>
        /// Find user by his username.
        /// </summary>
        /// <param name="request">Username.</param>
        /// <returns>User.</returns>
        /// <exception cref="DocParty.Exceptions.NotFoundException">
        /// If user with same name is not exist throw exception
        /// which be handle by NotFoundPageFilter.
        /// </exception>
        public async Task<User> Handle(HandlerData<UserRoute, User> request, CancellationToken cancellationToken)
        {
            User user = await Context.Users
               .FirstOrDefaultAsync(user => user.UserName == request.Data.UserName);

            if (user == null)
                throw new NotFoundException();

            return user;
        }
    }
}
