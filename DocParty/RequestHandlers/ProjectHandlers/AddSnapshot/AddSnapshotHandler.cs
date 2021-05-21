using DocParty.Models;
using DocParty.RequestHandlers.ProjectHandlers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocParty.RequestHandlers.AddSnapshot
{
    class AddSnapshotHandler : IRequestHandler<ProjectHandlerData<(string UserName,SnapshotFormData FormData), ErrorResponce>, ErrorResponce>
    {
        private const string InvalidSnapshotName = "This snapshot name is belongs to your other snapshot in this project";
        private ApplicationContext Context { get; }

        public AddSnapshotHandler(ApplicationContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<ErrorResponce> Handle(ProjectHandlerData<(string UserName, SnapshotFormData FormData), ErrorResponce> request, CancellationToken cancellationToken)
        {
            var errors = new List<string>();

            bool isExist = await Context.ProjectShapshots
                .Where(snapshot => snapshot.Project.Name == request.Project.Name
                    && snapshot.Project.Creator.UserName == request.Project.Creator.UserName)
                .AnyAsync(snapshot => snapshot.Name == request.Data.FormData.Name);

            if (isExist)
            {
                errors.Add(InvalidSnapshotName);
                return new ErrorResponce(errors);
            }

            User user = await Context.Users
                .FirstAsync(user => user.UserName == request.Data.UserName);

            var snapshot = new ProjectSnapshot
            {
                Name = request.Data.FormData.Name,
                Description = request.Data.FormData.Description,
                Project = request.Project,
                Author = user
            };

            await Context.ProjectShapshots.AddAsync(snapshot);
            await Context.SaveChangesAsync();

            return new ErrorResponce (errors);
        }
    }
}
