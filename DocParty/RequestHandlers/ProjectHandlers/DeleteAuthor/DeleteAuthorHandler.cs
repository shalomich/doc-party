using DocParty.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocParty.RequestHandlers.ProjectHandlers.DeleteAuthor
{
    class DeleteAuthorHandler : IRequestHandler<ProjectHandlerData<string, ErrorResponce>, ErrorResponce>
    {
        private ApplicationContext Context { get; }

        public DeleteAuthorHandler(ApplicationContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<ErrorResponce> Handle(ProjectHandlerData<string, ErrorResponce> request, CancellationToken cancellationToken)
        {
            string userName = request.Data;

            User user = await Context.Users
                .Include(user => user.ProjectRoles)
                .FirstAsync(user => user.UserName == userName);

            UserProjectRole projectRole = user.ProjectRoles
                .First(projectRole => projectRole.Project.Name == request.Project.Name);

            user.ProjectRoles = user.ProjectRoles.ToList();
            ((List<UserProjectRole>)user.ProjectRoles).Remove(projectRole);

            Context.Users.Update(user);
            await Context.SaveChangesAsync();

            return new ErrorResponce(new List<string>());
        }
    }
}
