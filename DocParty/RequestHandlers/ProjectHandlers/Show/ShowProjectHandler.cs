using DocParty.Models;
using DocParty.Services.Paginators;
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
        private Paginator<ProjectSnapshot,Comment> CommentPaginator { get; }
        private Paginator<Project, ProjectSnapshot> SnapshotPaginator { get; }

        public ShowProjectHandler(ApplicationContext context, Paginator<ProjectSnapshot, Comment> commentPaginator, Paginator<Project, ProjectSnapshot> snapshotPaginator)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            CommentPaginator = commentPaginator ?? throw new ArgumentNullException(nameof(commentPaginator));
            SnapshotPaginator = snapshotPaginator ?? throw new ArgumentNullException(nameof(snapshotPaginator));
        }

        public async Task<Project> Handle(HandlerData<Project, Project> request, CancellationToken cancellationToken)
        {
            return await Context.Projects
                .Include(project => project.Creator)
                .Include(project => project.Snapshots)
                    .ThenInclude(snapshot => snapshot.Comments)
                .FirstAsync(project => project.Name == request.Data.Name
                    && project.Creator.UserName == request.Data.Creator.UserName);
        }
    }
}
