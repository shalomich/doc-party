using DocParty.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocParty.RequestHandlers.Delete
{
    class DeleteProjectHandler : IRequestHandler<HandlerData<Project,Unit>, Unit>
    {
        private ApplicationContext Context { get; }

        public DeleteProjectHandler(ApplicationContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<Unit> Handle(HandlerData<Project, Unit> request, CancellationToken cancellationToken)
        {
            Context.Projects.Remove(request.Data);
            await Context.SaveChangesAsync();

            return Unit.Value;

        }
    }
}
