using DocParty.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocParty.RequestHandlers.ShowProject
{
    class ShowProjectHandler : IRequestHandler<HandlerData<Project, Project>, Project>
    {
        private ApplicationContext Context { get; }

        public ShowProjectHandler(ApplicationContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Project> Handle(HandlerData<Project, Project> request, CancellationToken cancellationToken)
        {
            var project = await Context.Projects
                .FirstAsync(project => project.Name == request.Data.Name
                    && project.Creator.UserName == request.Data.Creator.UserName);

            var snapshots = SnapshotPaginator.Get(project)
                .GetPage(Context.ProjectShapshots
                    .Where(snapshot => snapshot.Project.Name == request.Data.Name))
                .ToList();

            foreach (var snapshot in snapshots)
            {
                var comments = CommentPaginator.Get(snapshot)
                    .GetPage(Context.Comments
                        .Where(comment => comment.ProjectSnapshot.Name == snapshot.Name))
                    .ToList();
                snapshot.Comments = comments;
            }

            project.Snapshots = snapshots;

            return project;
        }
    }
}
