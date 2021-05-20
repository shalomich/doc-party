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
    class DeleteAuthorHandler : IRequestHandler<UserHandlerData<AuthorDeletingFormData, ErrorResponce>, ErrorResponce>
    {
        private ApplicationContext Context { get; }

        public DeleteAuthorHandler(ApplicationContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<ErrorResponce> Handle(UserHandlerData<AuthorDeletingFormData, ErrorResponce> request, CancellationToken cancellationToken)
        {
            User user = await Context.Users
                .Include(user => user.ProjectRoles)
                    .ThenInclude(projectRole => projectRole.Project)
                .FirstAsync(user => user.UserName == request.Data.UserName);

            UserProjectRole projectRole = user.ProjectRoles
                .First(projectRole => projectRole.Project.Name == request.Data.ProjectName);

            user.ProjectRoles = user.ProjectRoles.ToList();
            ((List<UserProjectRole>)user.ProjectRoles).Remove(projectRole);

            Context.Users.Update(user);
            await Context.SaveChangesAsync();

            return new ErrorResponce(new List<string>());
        }
    }
}
