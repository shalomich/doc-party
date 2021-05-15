using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocParty.RequestHandlers.ShowSnapshots
{
    class ShowSnapshotsHandler : IRequestHandler<HandlerData<UserRequest, IEnumerable<SnapshotData>>, IEnumerable<SnapshotData>>
    {
        private ApplicationContext Context { get; }
        public ShowSnapshotsHandler(ApplicationContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<IEnumerable<SnapshotData>> Handle(HandlerData<UserRequest, IEnumerable<SnapshotData>> request, CancellationToken cancellationToken)
        {
            return Context.ProjectShapshots
                .Where(snapshot => snapshot.Author.UserName == request.Data.UserName)
                .Select(snapshot => new SnapshotData
                {
                    SnapshotName = snapshot.Name,
                    ProjectName = snapshot.Project.Name,
                    CommentCount = snapshot.Comments.Count(),
                    Description = snapshot.Description
                }).ToList();
        }
    }
}
