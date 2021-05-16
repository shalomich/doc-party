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
    class ShowProjectHandler : IRequestHandler<HandlerData<ProjectRequest, Project>, Project>
    {
        private ApplicationContext Context { get; }

        public ShowProjectHandler(ApplicationContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<Project> Handle(HandlerData<ProjectRequest, Project> request, CancellationToken cancellationToken)
        {
            return await Context.Projects
                .Include(project => project.Creator)
                .Include(project => project.Snapshots)
                .FirstAsync(project => project.Name == request.Data.ProjectName
                    && project.Creator.UserName == request.Data.UserName);
        }
    }
}
