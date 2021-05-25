using DocParty.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocParty.Services
{
    class AuthorAssignService
    {
        private ApplicationContext Context { get; }
        private RoleManager<Role> RoleManager { get; }

        public AuthorAssignService(ApplicationContext context, RoleManager<Role> roleManager)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            RoleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        }

        public async Task Assign(UnauthorizedAuthor author)
        {
            bool isAlreadyInProject = Context.Users
                .Where(user => user.ProjectRoles
                    .Any(projectRole => projectRole.Project.Id == author.Project.Id))
                .Any(user => user.Email == author.Email);

            if (isAlreadyInProject)
            {
                return;
            }

            User user = await Context.Users
                .Include(user => user.ProjectRoles)
                .FirstOrDefaultAsync(user => user.Email == author.Email);

            if (user != null)
            {
                Role authorRole = await RoleManager.FindByNameAsync(Role.Value.Author.ToString());

                var projectRole = new UserProjectRole
                {
                    Project = author.Project,
                    Role = authorRole
                };

                user.ProjectRoles = user.ProjectRoles.ToList();
                ((List<UserProjectRole>)user.ProjectRoles).Add(projectRole);


                Context.Users.Update(user);
                await Context.SaveChangesAsync();
            }
            else
            {
                bool isAssigned = Context.UnauthorizedAuthors
                    .Any(unathorized => unathorized.Email == author.Email
                        && unathorized.ProjectId == author.Project.Id);

                if (isAssigned == false)
                {
                    Context.UnauthorizedAuthors.Add(author);
                    await Context.SaveChangesAsync();
                }
            }
        }

        public async Task CheckRegisteredUser(string email)
        {
            var unauthorizedAuthorProjects = Context.UnauthorizedAuthors
                .Include(author => author.Project)
                .Where(author => author.Email == email)
                .ToList();

            if (unauthorizedAuthorProjects.Any() != false)
            {
                foreach (var unauthorizedAuthorProject in unauthorizedAuthorProjects)
                {
                    await Assign(unauthorizedAuthorProject);
                    Context.UnauthorizedAuthors.Remove(unauthorizedAuthorProject);
                }
            }

            await Context.SaveChangesAsync();
        }
    }
}
