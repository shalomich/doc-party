using DocParty.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocParty.RequestHandlers.ProjectHandlers.ShowAuthors
{
    class ShowAuthorsHandler : IRequestHandler<HandlerData<Project, IEnumerable<User>>,IEnumerable<User>>
    {
        private ApplicationContext Context { get; }

        public ShowAuthorsHandler(ApplicationContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<User>> Handle(HandlerData<Project, IEnumerable<User>> request, CancellationToken cancellationToken)
        {
            return await Context.Users
                .Where(user => user.ProjectRoles
                    .Any(projectRole => projectRole.Project.Name == request.Data.Name)
                && user.UserName != request.Data.Creator.UserName)
                .ToListAsync();
        }
    }
}
