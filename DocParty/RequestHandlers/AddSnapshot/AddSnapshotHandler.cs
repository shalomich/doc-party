using DocParty.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocParty.RequestHandlers.AddSnapshot
{
    class AddSnapshotHandler : IRequestHandler<UserHandlerData<SnapshotFormData, ErrorResponce>, ErrorResponce>
    {
        private const string InvalidSnapshotName = "This snapshot name is belongs to your other snapshot in this project";
        private ApplicationContext Context { get; }

        public AddSnapshotHandler(ApplicationContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<ErrorResponce> Handle(UserHandlerData<SnapshotFormData, ErrorResponce> request, CancellationToken cancellationToken)
        {
            var errors = new List<string>();

            bool isExist = await Context.ProjectShapshots
                .Where(snapshot => snapshot.Project.Name == ((ProjectRequest)request.UserRequest).ProjectName)
                .AnyAsync(snapshot => snapshot.Name == request.Data.Name);

            if (isExist)
            {
                errors.Add(InvalidSnapshotName);
                return new ErrorResponce(errors);
            }

            Project project = await Context.Projects
                .FirstAsync(project => project.Name == ((ProjectRequest)request.UserRequest).ProjectName
                    && project.AuthorRoles.Any(userRole => userRole.User.UserName == request.UserRequest.UserName));

            User user = await Context.Users
                .FirstAsync(user => user.UserName == request.UserRequest.UserName);

            var snapshot = new ProjectSnapshot
            {
                Name = request.Data.Name,
                Description = request.Data.Description,
                Project = project,
                Author = user
            };

            await Context.ProjectShapshots.AddAsync(snapshot);
            await Context.SaveChangesAsync();

            return new ErrorResponce (errors);
        }
    }
}
