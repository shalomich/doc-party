using DocParty.Filters;
using DocParty.Models;
using DocParty.RequestHandlers;
using DocParty.RequestHandlers.CommentProject;
using DocParty.RequestHandlers.ProjectHandlers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.Controllers
{
    [Authorize]
    [Route("{userName}/{projectName}")]
    public class ProjectController : Controller
    {
        private readonly IMediator _mediator;

        private Project _project;

        public ProjectController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [ServiceFilter(typeof(PaginationFilter))]
        public async Task<IActionResult> Show([FromRoute] ProjectRoute route)
        {
            await Init(route);

            var request = new HandlerData<Project, Project>
            {
                Data = _project
            };

            Project projectWithSnapshots = await _mediator.Send(request);

            return View("Project", projectWithSnapshots);
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
        public async Task<IActionResult> AddSnapshot([FromForm] SnapshotFormData formData, [FromRoute] ProjectRoute route)
        {
            await Init(route);

            string currentUserName = User.Identity.Name;

            var request = new ProjectHandlerData<SnapshotFormData, ErrorResponce>
            {
                Project = _project,
                Data = formData
            };

            ErrorResponce responce = await _mediator.Send(request);

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

            ErrorResponce responce = await _mediator.Send(request);

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
        public async Task<IActionResult> Comment([FromForm] CommentFormData formData, [FromRoute] ProjectRoute route)
        {
            await Init(route);

            var request = new ProjectHandlerData<CommentFormData, ErrorResponce>
            {
                Data = formData, 
                Project = _project,
            };

            ErrorResponce responce = await _mediator.Send(request);

            return RedirectToAction(nameof(Show), route);

        }
    }
}
