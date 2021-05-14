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

        [HttpPost]
        [Route("deletion")]
        public async Task<IActionResult> DeleteProject([FromRoute] string userName, [FromRoute] string projectName)
        {
            var request = new HandlerData<ProjectRequest, ErrorResponce>
            {
                Data = new ProjectRequest { UserName = userName, ProjectName = projectName },
            };

            ErrorResponce responce = await _mediator.Send(request);

            return RedirectToRoute("projects",new {userName = userName});
        }
    }
}
