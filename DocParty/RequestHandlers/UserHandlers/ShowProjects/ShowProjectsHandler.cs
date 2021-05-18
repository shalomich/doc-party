using DocParty.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocParty.RequestHandlers.Projects
{
    class ShowProjectsHandler : IRequestHandler<HandlerData<User,IEnumerable<ProjectTableData>>,IEnumerable<ProjectTableData>>
    {
        private ApplicationContext Context { get; }

        public ShowProjectsHandler(ApplicationContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }
        
        public async Task<IEnumerable<ProjectTableData>> Handle(HandlerData<User, IEnumerable<ProjectTableData>> request, CancellationToken cancellationToken)
        {
            return await Context.Projects
                                   .Where(project => project.AuthorRoles.Any(authorRole => authorRole.User.UserName == request.Data.UserName))
                                   .Select(project => new ProjectTableData
                                   {
                                       ProjectName = project.Name,
                                       CreatorName = project.Creator.UserName,
                                       AuthorCount = project.AuthorRoles.Count(),
                                       IsActive = project.isActive,
                                       SnapshotCount = project.Snapshots.Count()
                                   })
                                   .ToListAsync();
        }
    }


}
