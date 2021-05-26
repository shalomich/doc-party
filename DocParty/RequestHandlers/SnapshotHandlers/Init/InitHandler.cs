using DocParty.Exceptions;
using DocParty.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocParty.RequestHandlers.SnapshotHandlers.Init
{
    class InitHandler : IRequestHandler<HandlerData<SnapshotRoute, ProjectSnapshot>, ProjectSnapshot>
    {
        private ApplicationContext Context { get; }

        public InitHandler(ApplicationContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Find snapshot by his name, project name and creator name.
        /// </summary>
        /// <param name="request">User name, snapshot name and project name.</param>
        /// <returns>Snapshot.</returns>
        /// <exception cref="DocParty.Exceptions.NotFoundException">
        /// If snapshot with same name, project name and creator name is not exist throw exception
        /// which be handle by NotFoundPageFilter.
        /// </exception>
        public async Task<ProjectSnapshot> Handle(HandlerData<SnapshotRoute, ProjectSnapshot> request, CancellationToken cancellationToken)
        {
            ProjectSnapshot snapshot = await Context.ProjectShapshots
                .Include(snapshot => snapshot.Project)
                .FirstOrDefaultAsync(snapshot => snapshot.Name == request.Data.SnapshotName
                    && snapshot.Project.Name == request.Data.ProjectName
                    && snapshot.Project.Creator.UserName == request.Data.UserName);

            if (snapshot == null)
                throw new NotFoundException();

            return snapshot;
        }
    }
}
