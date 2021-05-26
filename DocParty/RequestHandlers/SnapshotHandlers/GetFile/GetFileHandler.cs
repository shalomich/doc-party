using DocParty.Models;
using DocParty.RequestHandlers.SnapshotHandlers.ShowFile;
using DocParty.Services.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DocParty.RequestHandlers.SnapshotHandlers.ShowFileHandler
{
    class ShowFileHandler : IRequestHandler<HandlerData<ProjectSnapshot, FileData>, FileData>
    {
        private IRepository<byte[],string> Repository { get; }
        public ShowFileHandler(IRepository<byte[], string> repository, ApplicationContext context)
        {
            Repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// Get file for snapshot from repository.
        /// </summary>
        /// <param name="request">Snapshot.</param>
        /// <returns>Array of file bytes and file content type.</returns>

        public async Task<FileData> Handle(HandlerData<ProjectSnapshot, FileData> request, CancellationToken cancellationToken)
        {
            
            string name = request.Data.Id.ToString();
            string fileName = FileData.GetFileName(name,request.Data.Project.FileContentType);

            
            byte[] fileBytes = Repository.Select(fileName);

            return new FileData
            {
                Bytes = fileBytes,
                ContentType = request.Data.Project.FileContentType
            };        
        }
    }
}
