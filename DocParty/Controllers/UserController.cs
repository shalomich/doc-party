
using DocParty.RequestHandlers;
using DocParty.RequestHandlers.Profile;
using DocParty.RequestHandlers.Projects;
using DocParty.ViewModel;
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
    public class UserController : Controller
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [Route("{userName}")]
        public async Task<IActionResult> ShowProfile([FromRoute] UserQuery<UserProfile> query)
        {
            UserProfile statistics = await _mediator.Send(query);

            return View("Profile", statistics);
        }

        [Route("{userName}/projects")]
        public async Task<IActionResult> ShowProjects([FromRoute] UserQuery<IEnumerable<ProjectData>> query)
        {
            IEnumerable<ProjectData> data = await _mediator.Send(query);

            return View("Projects", new ProjectsInfo { Data = data, UserName = query.UserName});
        }

        
        
    }
}
