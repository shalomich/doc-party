using DocParty.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocParty.RequestHandlers.ShowProject
{
    class ShowProjectHandler : IRequestHandler<HandlerData<Project, Project>, Project>
    {
        private ApplicationContext Context { get; }

        public ShowProjectHandler(ApplicationContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Project> Handle(HandlerData<Project, Project> request, CancellationToken cancellationToken)
        {
            return await Context.Projects
                .Include(project => project.Creator)
                .Include(project => project.Snapshots)
                    .ThenInclude(snapshot => snapshot.Comments)
                .FirstAsync(project => project.Name == request.Data.Name
                    && project.Creator.UserName == request.Data.Creator.UserName);
        }
    }
}
