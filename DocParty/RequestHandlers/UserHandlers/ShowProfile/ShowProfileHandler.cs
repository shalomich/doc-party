using DocParty.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocParty.RequestHandlers.Profile
{
    class ShowProfileHandler : IRequestHandler<HandlerData<User,UserStatistics>, UserStatistics>
    {
        private ApplicationContext Context { get; }

        public ShowProfileHandler(ApplicationContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Get user project statistics.
        /// </summary>
        /// <param name="request">User.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>User project statistics.</returns>
        public async Task<UserStatistics> Handle(HandlerData<User, UserStatistics> request, CancellationToken cancellationToken)
        {
            string userName = request.Data.UserName;

            var allUserProjects = Context.Projects
                    .Where(project => project.AuthorRoles.Any(authorRole => authorRole.User.UserName == userName));

            var createdByUserProjects = Context.Projects
                    .Where(project => project.Creator.UserName == userName);

            var allUserProjectSnapshots = Context.ProjectShapshots
                    .Where(snapshot => snapshot.Project.Creator.UserName == userName);

            var createdByUserSnapshots = Context.ProjectShapshots
                    .Where(snapshot => snapshot.Author.UserName == userName);

            var writtenByUserComments = Context.Comments
                    .Where(comment => comment.Author.UserName == userName);

            var allUserProjectComments = Context.Comments
                    .Where(comment => comment.ProjectSnapshot.Project.Creator.UserName == userName);

            return new UserStatistics
            {
                AllProjectCount = allUserProjects.Count(),
                CreatedProjectCount = createdByUserProjects.Count(),
                ClosedAllProjectCount = allUserProjects.Where(project => project.isActive == false).Count(),
                ClosedCreatedProjectCount = createdByUserProjects.Where(project => project.isActive == false).Count(),
                AllUserProjectSnapshotsCount = allUserProjectSnapshots.Count(),
                CreatedSnapshotsCount = createdByUserSnapshots.Count(),
                AllUserProjectCommentCount = allUserProjectSnapshots.Count(),
                WrittenCommentCount = writtenByUserComments.Count()
            };
        }
    }
}
