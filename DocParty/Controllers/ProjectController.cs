using DocParty.Extensions;
using DocParty.Filters;
using DocParty.Models;
using DocParty.RequestHandlers;
using DocParty.RequestHandlers.CommentProject;
using DocParty.RequestHandlers.ProjectHandlers;
using DocParty.Services.Tables;
using DocParty.ViewModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.Controllers
{
    [Authorize]
    [Route("{userName}/{projectName}")]
    [NotFoundPageFilter]
    public class ProjectController : Controller
    {
        private readonly IMediator _mediator;

        private Project _project;

        public ProjectController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [RestoreModelStateFromTempData]
        public async Task<IActionResult> Show([FromRoute] ProjectRoute route)
        {
            await Init(route);

            var request = new HandlerData<Project, Dictionary<ProjectSnapshotsTableRow,IEnumerable<string>>>
            {
                Data = _project
            };

            Dictionary<ProjectSnapshotsTableRow,IEnumerable<string>> dataAndComments = await _mediator.Send(request);

            string [] snapshotNames = dataAndComments.Select(data => data.Key.Name).ToArray();
            string [] snaspshotReferences = dataAndComments.Select(data =>
                $"{HttpContext.Request.Path}/{data.Key.Name}")
                .ToArray(); 

            var table = new ReferencedTable(
                new NumberedTable(new ObjectTable<ProjectSnapshotsTableRow>(dataAndComments
                    .Select(data => data.Key).ToArray())),
                new Dictionary<string, string[]>()
                {
                    {"Name", snaspshotReferences}
                }
            );

            return View("Project", new ProjectInfo 
            {
                Name = _project.Name,
                IsActive = _project.isActive,
                SnapshotAddingLocation = HttpContext.Request.Path,
                StateLocation = $"{HttpContext.Request.Path}/state",
                FormData = new CommentAddingFormData
                {
                    CommentLocation = $"{HttpContext.Request.Path}/comment",
                    SnapshotChoice = new SelectList(snapshotNames)
                },
                Data = new ProjectSnapshotsTableData
                {
                    Table = table,
                    Comments = dataAndComments.Select(data => data.Value).ToArray()
                }
            });
        }

        private async Task Init(ProjectRoute route)
        {
            
            var request = new HandlerData<ProjectRoute, Project>
            {
                Data = route
            };

            _project = await _mediator.Send(request);
        }

        [HttpPost]
        [SetTempDataModelState]
        public async Task<IActionResult> AddSnapshot([FromForm] SnapshotFormData formData, [FromRoute] ProjectRoute route)
        {
            if (ModelState.IsValid)
            {
                await Init(route);

                var request = new ProjectHandlerData<(string UserName, SnapshotFormData FormData), ErrorResponce>
                {
                    Project = _project,
                    Data = (User.Identity.Name, formData)
                };

                ErrorResponce responce = await _mediator.Send(request);

                ModelState.CheckErrors(responce);
            }

            return RedirectToAction(nameof(Show), route);
        }

        [HttpPost]
        [Route("deletion")]
        public async Task<IActionResult> Delete([FromRoute] ProjectRoute route)
        {
            await Init(route);

            var request = new HandlerData<Project, ErrorResponce>
            {
                Data = _project
            };

            await _mediator.Send(request);

            return RedirectToRoute("Projects", new { userName = route.UserName});
        }

        [HttpPost]
        [Route("state")]
        
        public async Task<IActionResult> ChangeState([FromRoute] ProjectRoute route)
        {
            await Init(route);

            var request = new HandlerData<Project, Unit>
            {
                Data = _project,
            };

            await _mediator.Send(request);

            return RedirectToAction(nameof(Show), route);
        }

        [HttpPost]
        [Route("comment")]
        [SetTempDataModelState]
        public async Task<IActionResult> Comment([FromForm] CommentFormData formData, [FromRoute] ProjectRoute route)
        {
            if (ModelState.IsValid)
            {
                await Init(route);

                var request = new ProjectHandlerData<(string UserName, CommentFormData FormData), ErrorResponce>
                {
                    Data = (User.Identity.Name, formData),
                    Project = _project,
                };

                ErrorResponce responce = await _mediator.Send(request);

                ModelState.CheckErrors(responce);
            }
            
            return RedirectToAction(nameof(Show), route);

        }
    }
}
