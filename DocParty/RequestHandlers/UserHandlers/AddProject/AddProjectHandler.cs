using DocParty.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DocParty.RequestHandlers.UserHandlers;
using Microsoft.AspNetCore.Identity;

namespace DocParty.RequestHandlers.AddProject
{
    class AddProjectHandler : IRequestHandler<UserHandlerData<SnapshotFormData,ErrorResponce>, ErrorResponce>
    {
        private const string InvalidProjectName = "This project name is belongs to your other project";
        private ApplicationContext Context { get; }
        private RoleManager<Role> RoleManager { get; }

        public AddProjectHandler(ApplicationContext context, RoleManager<Role> roleManager)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            RoleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        }

        public async Task<ErrorResponce> Handle(UserHandlerData<SnapshotFormData, ErrorResponce> request, CancellationToken cancellationToken)
        { 
            var errors = new List<string>();

            bool isExist = Context.Projects
                .Where(project => project.Creator.UserName == request.User.UserName)
                .Any(project => project.Name == request.Data.Name);

            if (isExist)
            {
                errors.Add(InvalidProjectName);
                return new ErrorResponce(errors);
            }
            
            var project = new Project(request.Data.Name, request.Data.Description);

            Role creatorRole = await RoleManager.FindByNameAsync(Role.Value.Creator.ToString());
             
            var user = await Context.Users
                .Include(user => user.ProjectRoles)
                .FirstAsync(user => user.UserName == request.User.UserName);

            project.Creator = user;

            user.ProjectRoles = user.ProjectRoles.ToList();
            ((List<UserProjectRole>)user.ProjectRoles).Add(new UserProjectRole
            {
                Project = project,
                Role = creatorRole
            });


            Context.Users.Update(user);
            await Context.SaveChangesAsync();

            return new ErrorResponce(errors);
        }
    }
}
