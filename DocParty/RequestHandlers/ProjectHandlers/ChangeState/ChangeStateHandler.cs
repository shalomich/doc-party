using DocParty.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocParty.RequestHandlers.ChangeState
{
    class ChangeStateHandler : IRequestHandler<HandlerData<ProjectRequest, Unit>, Unit>
    {
        private ApplicationContext Context { get; }

        public ChangeStateHandler(ApplicationContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<Unit> Handle(HandlerData<ProjectRequest, Unit> request, CancellationToken cancellationToken)
        {
            Project project = await Context.Projects
                .FirstAsync(project => project.Name == request.Data.ProjectName
                    && project.Creator.UserName == request.Data.UserName);

            project.isActive = !project.isActive;

            Context.Projects.Update(project);

            await Context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
