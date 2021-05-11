
using DocParty.RequestHandlers;
using DocParty.RequestHandlers.UserProfile;
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
    public class UserProfileController : Controller
    {
        private readonly IMediator _mediator;

        public UserProfileController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [Route("{userName}")]
        public async Task<IActionResult> GetUserProfile([FromRoute] UserQuery query)
        {
            UserStatistics statistics = await _mediator.Send(query);

            return View("Profile", statistics);
        }
    }
}
