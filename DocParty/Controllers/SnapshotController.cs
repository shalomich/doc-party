using DocParty.Filters;
using DocParty.Models;
using DocParty.RequestHandlers;
using DocParty.RequestHandlers.SnapshotHandlers;
using DocParty.RequestHandlers.SnapshotHandlers.ShowFile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.Controllers
{
    [Authorize]
    [Route("{userName}/{projectName}/{snapshotName}")]
    [NotFoundPageFilter]
    public class SnapshotController : Controller
    {
        private ProjectSnapshot _snapshot;

        private IMediator _mediator;

        public SnapshotController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        private async Task Init(SnapshotRoute route)
        {
            var request = new HandlerData<SnapshotRoute, ProjectSnapshot>
            {
                Data = route
            };

            _snapshot = await _mediator.Send(request);
        }

        public async Task<IActionResult> ShowFile(SnapshotRoute route)
        {
            FileData data = await GetFile(route);

            return File(data.Bytes,data.ContentType);
        }

        [Route("download")]
        [HttpPost]
        public async Task<IActionResult> DownloadFile(SnapshotRoute route)
        {
            FileData data = await GetFile(route);

            string fileName = FileData.GetFileName(_snapshot.Name, data.ContentType);
            
            return File(data.Bytes, data.ContentType, fileName);
        }

        private async Task<FileData> GetFile(SnapshotRoute route)
        {
            await Init(route);

            var request = new HandlerData<ProjectSnapshot, FileData>
            {
                Data = _snapshot
            };

            return await _mediator.Send(request);
        }
    }
}
