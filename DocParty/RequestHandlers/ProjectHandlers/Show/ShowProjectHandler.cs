using DocParty.Models;
using DocParty.ViewModel;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocParty.RequestHandlers.ShowProject
{
    class ShowProjectHandler : IRequestHandler<HandlerData<Project, Dictionary<ProjectSnapshotsTableRow, IEnumerable<string>>>, Dictionary<ProjectSnapshotsTableRow, IEnumerable<string>>>
    {
        private const string CommentTemplate = "{0}: {1}";
        private ApplicationContext Context { get; }

        public ShowProjectHandler(ApplicationContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<Dictionary<ProjectSnapshotsTableRow, IEnumerable<string>>> Handle(HandlerData<Project, Dictionary<ProjectSnapshotsTableRow, IEnumerable<string>>> request, CancellationToken cancellationToken)
        {
            return await Context.ProjectShapshots
             .Include(snapshot => snapshot.Comments)
             .Include(snapshot => snapshot.Author)
             .Where(snapshot => snapshot.Project.Name == request.Data.Name
                 && snapshot.Project.Creator.UserName == request.Data.Creator.UserName)
             .ToDictionaryAsync(snapshot =>
                new ProjectSnapshotsTableRow
                {
                    Name = snapshot.Name,
                    AuthorName = snapshot.Author.UserName,
                    Description = snapshot.Description,
                },
                snapshot => snapshot.Comments.Select(comment => 
                    String.Format(CommentTemplate, comment.Author.UserName, comment.Text)));
        }
    }
}
