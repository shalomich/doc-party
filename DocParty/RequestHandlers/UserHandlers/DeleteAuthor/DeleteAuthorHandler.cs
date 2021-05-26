using DocParty.Models;
using DocParty.RequestHandlers.UserHandlers;
using DocParty.RequestHandlers.UserHandlers.DeleteAuthor;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocParty.RequestHandlers.ProjectHandlers.DeleteAuthor
{
    class DeleteAuthorHandler : IRequestHandler<UserHandlerData<AuthorDeletingFormData, Unit>, Unit>
    {
        private ApplicationContext Context { get; }

        public DeleteAuthorHandler(ApplicationContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Delete author from project.
        /// </summary>
        /// <param name="request">Author name and project name.</param>
        /// <returns> Nothing.</returns>
        public async Task<Unit> Handle(UserHandlerData<AuthorDeletingFormData, Unit> request, CancellationToken cancellationToken)
        {
            User user = await Context.Users
                .Include(user => user.ProjectRoles)
                    .ThenInclude(projectRole => projectRole.Project)
                .FirstAsync(user => user.UserName == request.Data.UserName);

            UserProjectRole projectRole = user.ProjectRoles
                .First(projectRole => projectRole.Project.Name == request.Data.ProjectName
                    && projectRole.Project.Creator.UserName == request.User.UserName);

            /// Deleting user role author for current project. 
            user.ProjectRoles = user.ProjectRoles.ToList();
            ((List<UserProjectRole>)user.ProjectRoles).Remove(projectRole);

            Context.Users.Update(user);
            await Context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
