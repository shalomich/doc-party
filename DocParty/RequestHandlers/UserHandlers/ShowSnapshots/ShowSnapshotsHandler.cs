using DocParty.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocParty.RequestHandlers.ShowSnapshots
{
    class ShowSnapshotsHandler : IRequestHandler<HandlerData<User, IEnumerable<SnapshotTableData>>, IEnumerable<SnapshotTableData>>
    {
        private ApplicationContext Context { get; }
        public ShowSnapshotsHandler(ApplicationContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Get data about snapshot that will be in table.
        /// </summary>
        /// <param name="request">User.</param>
        /// <returns>Collection of table row data.</returns>
        public async Task<IEnumerable<SnapshotTableData>> Handle(HandlerData<User, IEnumerable<SnapshotTableData>> request, CancellationToken cancellationToken)
        {
            return await Context.ProjectShapshots
                .Where(snapshot => snapshot.Author.UserName == request.Data.UserName)
                .Select(snapshot => new SnapshotTableData
                {
                    SnapshotName = snapshot.Name,
                    ProjectName = snapshot.Project.Name,
                    CommentCount = snapshot.Comments.Count(),
                    Description = snapshot.Description
                }).ToListAsync();
        }
    }
}
