using DocParty.Exceptions;
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

        /// <summary>
        /// Find project by his name and creator name.
        /// </summary>
        /// <param name="request">User name and project name.</param>
        /// <returns>Project.</returns>
        /// <exception cref="DocParty.Exceptions.NotFoundException">
        /// If project with same name and creator name is not exist throw exception
        /// which be handle by NotFoundPageFilter.
        /// </exception>
        public async Task<Project> Handle(HandlerData<ProjectRoute, Project> request, CancellationToken cancellationToken)
        {
            Project project = await Context.Projects
               .Include(project => project.Creator)
               .FirstOrDefaultAsync(project => project.Name == request.Data.ProjectName
                   && project.Creator.UserName == request.Data.UserName);

            if (project == null)
                throw new NotFoundException();

            return project;
        }
    }
}
