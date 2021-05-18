using DocParty.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocParty.RequestHandlers.ProjectHandlers.Init
{
    class InitHandler : IRequestHandler<HandlerData<ProjectRoute, Project>,Project>
    {
        private ApplicationContext Context { get; }

        public InitHandler(ApplicationContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Project> Handle(HandlerData<ProjectRoute, Project> request, CancellationToken cancellationToken)
        {
            return await Context.Projects
               .Include(project => project.Creator)
               .FirstAsync(project => project.Name == request.Data.ProjectName
                   && project.Creator.UserName == request.Data.UserName);
        }
    }
}
