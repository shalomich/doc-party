using DocParty.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocParty.RequestHandlers.Delete
{
    class DeleteProjectHandler : IRequestHandler<HandlerData<ProjectRequest,ErrorResponce>, ErrorResponce>
    {
        private ApplicationContext Context { get; }

        public DeleteProjectHandler(ApplicationContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<ErrorResponce> Handle(HandlerData<ProjectRequest, ErrorResponce> request, CancellationToken cancellationToken)
        {
            Project project = await Context.Projects
            .Where(project => project.Creator.UserName == request.Data.UserName)
            .FirstOrDefaultAsync(project => project.Name == request.Data.ProjectName);

            Context.Projects.Remove(project);
            Context.SaveChanges();

            return new ErrorResponce(new List<string>());

        }
    }
}
