using DocParty.Models;
using DocParty.Services.Paginators;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.Filters
{
    class PaginationFilter : IActionFilter
    {
        public static string UserName;
        public static string ProjectName;
        private ApplicationContext Context { get; }
        private Paginator<ProjectSnapshot, Comment> CommentPaginator { get; }
        private Paginator<Project, ProjectSnapshot> SnapshotPaginator { get; }

        public PaginationFilter(ApplicationContext context, Paginator<ProjectSnapshot, Comment> commentPaginator, Paginator<Project, ProjectSnapshot> snapshotPaginator)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            CommentPaginator = commentPaginator ?? throw new ArgumentNullException(nameof(commentPaginator));
            SnapshotPaginator = snapshotPaginator ?? throw new ArgumentNullException(nameof(snapshotPaginator));
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            context.RouteData.Values.TryGetValue(nameof(UserName), out object userName);
            context.RouteData.Values.TryGetValue(nameof(ProjectName), out object projectName);

            if (UserName != (string)userName || ProjectName != (string)projectName)
            {
                UserName = (string) userName;
                ProjectName = (string)projectName;
                InitPaginators();
            }

        }

        private void InitPaginators()
        {
            var project = Context.Projects
                .Include(project => project.Snapshots)
                .First(project => project.Name == ProjectName);

            SnapshotPaginator.Create(project);

            foreach (var snapshot in project.Snapshots)
                CommentPaginator.Create(snapshot);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
          
        }

        
    }
}
