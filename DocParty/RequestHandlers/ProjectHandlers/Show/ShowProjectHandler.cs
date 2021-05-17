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
    class ShowProjectHandler : IRequestHandler<HandlerData<ProjectRequest, Project>, Project>
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

        public async Task<Project> Handle(HandlerData<ProjectRequest, Project> request, CancellationToken cancellationToken)
        {
            return await Context.Projects
                .Include(project => project.Creator)
                .Include(project => SnapshotPaginator.Get(project).GetPage(project.Snapshots.AsQueryable()))
                    .ThenInclude(snapshot => CommentPaginator.Get(snapshot).GetPage(snapshot.Comments.AsQueryable()))
                .FirstAsync(project => project.Name == request.Data.ProjectName
                    && project.Creator.UserName == request.Data.UserName);
        }
    }
}
