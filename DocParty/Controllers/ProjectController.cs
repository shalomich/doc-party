using DocParty.Models;
using DocParty.RequestHandlers;
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

        public ProjectController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IActionResult> Show([FromRoute] string userName, [FromRoute] string projectName)
        {
            var request = new HandlerData<ProjectRequest, Project>
            {
                Data = new ProjectRequest { UserName = userName, ProjectName = projectName },
            };

            Project project = await _mediator.Send(request);

            return View("Project", project);
        }

        [HttpPost]
        public async Task<IActionResult> AddSnapshot([FromRoute] string userName, [FromRoute] string projectName, [FromForm] SnapshotFormData formData)
        {
            string currentUserName = User.Identity.Name;

            var request = new UserHandlerData<SnapshotFormData, ErrorResponce>
            {
                UserRequest = new ProjectRequest { UserName = currentUserName, ProjectName = projectName },
                Data = formData
            };

            ErrorResponce responce = await _mediator.Send(request);

            return RedirectToAction(nameof(Show), new { userName = userName, projectName = projectName });
        }

        [HttpPost]
        [Route("deletion")]
        public async Task<IActionResult> Delete([FromRoute] string userName, [FromRoute] string projectName)
        {
            var request = new HandlerData<ProjectRequest, ErrorResponce>
            {
                Data = new ProjectRequest { UserName = userName, ProjectName = projectName },
            };

            ErrorResponce responce = await _mediator.Send(request);

            return RedirectToRoute("Projects",new {userName = userName});
        }

        [HttpPost]
        [Route("state")]
        
        public async Task<IActionResult> ChangeState([FromRoute] string userName, [FromRoute] string projectName)
        {
            var request = new HandlerData<ProjectRequest, Unit>
            {
                Data = new ProjectRequest { UserName = userName, ProjectName = projectName },
            };

            await _mediator.Send(request);

            return RedirectToAction(nameof(Show), new { userName = userName, projectName = projectName});
        }
    }
}
