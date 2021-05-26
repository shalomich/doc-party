using DocParty.Models;
using DocParty.RequestHandlers.SnapshotHandlers.ShowFile;
using DocParty.Services.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocParty.RequestHandlers.Delete
{
    class DeleteProjectHandler : IRequestHandler<HandlerData<Project,ErrorResponce>, ErrorResponce>
    {
        private ApplicationContext Context { get; }
        private IRepository<byte[],string> Repository { get; }

        public DeleteProjectHandler(ApplicationContext context, IRepository<byte[], string> repository)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ErrorResponce> Handle(HandlerData<Project, ErrorResponce> request, CancellationToken cancellationToken)
        {
            await Context.ProjectShapshots
                .Where(snapshot => snapshot.ProjectId == request.Data.Id)
                .ForEachAsync(snapshot =>
                {
                    var thread = new Thread(new ThreadStart(() => 
                    {
                        string fileName = FileData.GetFileName(snapshot.Id.ToString(), request.Data.FileContentType);
                        Repository.Delete(fileName);
                    }));
                    thread.Start();
                });
                
            Context.Projects.Remove(request.Data);
            await Context.SaveChangesAsync();

            return new ErrorResponce(new List<string>());

        }
    }
}
