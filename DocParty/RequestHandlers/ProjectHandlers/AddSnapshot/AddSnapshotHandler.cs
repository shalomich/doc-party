using DocParty.Models;
using DocParty.RequestHandlers.ProjectHandlers;
using DocParty.RequestHandlers.SnapshotHandlers.ShowFile;
using DocParty.Services.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocParty.RequestHandlers.AddSnapshot
{
    class AddSnapshotHandler : IRequestHandler<ProjectHandlerData<(string UserName,SnapshotFormData FormData), ErrorResponce>, ErrorResponce>
    {
        private const string InvalidSnapshotNameMessage = "This snapshot name is belongs to your other snapshot in this project";
        private const string InvalidExtensionMessage = "This file has other extension";
        private ApplicationContext Context { get; }

        private IRepository<byte[],string> Repository { get; }

        public AddSnapshotHandler(ApplicationContext context, IRepository<byte[], string> repository)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ErrorResponce> Handle(ProjectHandlerData<(string UserName, SnapshotFormData FormData), ErrorResponce> request, CancellationToken cancellationToken)
        {
            var errors = new List<string>();

            bool isExist = await Context.ProjectShapshots
                .Where(snapshot => snapshot.Project.Name == request.Project.Name
                    && snapshot.Project.Creator.UserName == request.Project.Creator.UserName)
                .AnyAsync(snapshot => snapshot.Name == request.Data.FormData.Name);

            if (isExist)
            {
                errors.Add(InvalidSnapshotNameMessage);
                return new ErrorResponce(errors);
            }

            bool hasOtherContentType = request.Project.FileContentType != request.Data.FormData.File.ContentType;
            
            if (hasOtherContentType) 
            {
                errors.Add(InvalidExtensionMessage);
                return new ErrorResponce(errors);
            }

            User user = await Context.Users
                .FirstAsync(user => user.UserName == request.Data.UserName);

            var snapshot = new ProjectSnapshot
            {
                Name = request.Data.FormData.Name,
                Description = request.Data.FormData.Description,
                Project = request.Project,
                Author = user
            };

            await Context.ProjectShapshots.AddAsync(snapshot);
            await Context.SaveChangesAsync();

            byte[] fileBytes;
            using (var stream = new MemoryStream())
            {
                request.Data.FormData.File.CopyTo(stream);
                fileBytes = stream.ToArray();
            };

            string fileName = FileData.GetFileName(snapshot.Id.ToString(), request.Project.FileContentType);

            Repository.Create(fileName, fileBytes);

            return new ErrorResponce (errors);
        }
    }
}
