using DocParty.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocParty.RequestHandlers.ProjectHandlers.AddAuthor
{
    class AddAuthorHandler : IRequestHandler<ProjectHandlerData<AuthorFormData, ErrorResponce>, ErrorResponce>
    {
        private const string AlreadyInProjectMessage = "This user is already in project";
        private const string NotExistMessage = "This user does not exist";
        private ApplicationContext Context { get; }

        public AddAuthorHandler(ApplicationContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<ErrorResponce> Handle(ProjectHandlerData<AuthorFormData, ErrorResponce> request, CancellationToken cancellationToken)
        {
            var errors = new List<string>();

            bool isAlreadyInProject = Context.Users
                .Where(user => user.ProjectRoles
                    .Any(projectRole => projectRole.Project.Name == request.Project.Name))
                .Any(user => user.Email == request.Data.Email);

            if (isAlreadyInProject)
            {
                errors.Add(AlreadyInProjectMessage);
                return new ErrorResponce(errors);
            }

            User user = await Context.Users
                .Include(user => user.ProjectRoles)
                .FirstOrDefaultAsync(user => user.Email == request.Data.Email);

            if (user == null)
            {
                errors.Add(NotExistMessage);
                return new ErrorResponce(errors);
            }

            var projectRole = new UserProjectRole
            {
                Project = request.Project,
                Role = new Role { Name = Role.Value.Author.ToString() }
            };

            user.ProjectRoles = user.ProjectRoles.ToList();
            ((List<UserProjectRole>)user.ProjectRoles).Add(projectRole);


            Context.Users.Update(user);
            await Context.SaveChangesAsync();

            return new ErrorResponce(errors);
        }
    }
}
