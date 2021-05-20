using DocParty.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocParty.RequestHandlers.UserHandlers.AddAuthor
{
    class AddAuthorHandler : IRequestHandler<UserHandlerData<AuthorAddingFormData, ErrorResponce>, ErrorResponce>
    {
        private const string AlreadyInProjectMessage = "This user is already in project";
        private const string NotExistMessage = "This user does not exist";
        private ApplicationContext Context { get; }
        private RoleManager<Role> RoleManager { get; }

        public AddAuthorHandler(ApplicationContext context, RoleManager<Role> roleManager)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            RoleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        }

        public async Task<ErrorResponce> Handle(UserHandlerData<AuthorAddingFormData, ErrorResponce> request, CancellationToken cancellationToken)
        {
            var errors = new List<string>();

            bool isAlreadyInProject = Context.Users
                .Where(user => user.ProjectRoles
                    .Any(projectRole => projectRole.Project.Name == request.Data.ProjectName))
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

            Project project = Context.Projects
                .First(project => project.Name == request.Data.ProjectName
                    && project.Creator.UserName == request.User.UserName);


            Role authorRole = await RoleManager.FindByNameAsync(Role.Value.Author.ToString());

            var projectRole = new UserProjectRole
            {
                Project = project,
                Role = authorRole
            };

            user.ProjectRoles = user.ProjectRoles.ToList();
            ((List<UserProjectRole>)user.ProjectRoles).Add(projectRole);


            Context.Users.Update(user);
            await Context.SaveChangesAsync();

            return new ErrorResponce(errors);
        }
    }
}
