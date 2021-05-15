
using DocParty.Models;
using DocParty.RequestHandlers;
using DocParty.RequestHandlers.AddProject;
using DocParty.RequestHandlers.Profile;
using DocParty.RequestHandlers.Projects;
using DocParty.RequestHandlers.ShowSnapshots;
using DocParty.Services.Tables;
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
    [Route("{userName}")]
    public class UserController : Controller
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
  
        public async Task<IActionResult> ShowProfile([FromRoute] string userName)
        {
            var request = new HandlerData<UserRequest, UserProfile> { Data = new UserRequest { UserName = userName } };
            
            UserProfile statistics = await _mediator.Send(request);
            
            return View("Profile", statistics);
        }

        [Route("projects")]
        public async Task<IActionResult> ShowProjects([FromRoute] string userName)
        {
            var request = new HandlerData<UserRequest, IEnumerable<ProjectData>> { Data = new UserRequest { UserName = userName } };

            IEnumerable<ProjectData> data = await _mediator.Send(request);

            var columnReferenceTemplates = new Dictionary<string, string>
            {
                {"ProjectName", $"/{userName}/{{0}}" }
            };

            var table = new ReferencedTable(
                new NumberedTable(new ObjectTable<ProjectData>(data)),
                columnReferenceTemplates
            );  
            return View("Projects", new ProjectsInfo {Table = table,UserName = userName});
        }

        [HttpPost]
        [Route("projects")]
        public async Task<IActionResult> AddProject([FromRoute] string userName, [FromForm] AddingFormData data)
        {
            var request = new UserHandlerData<AddingFormData, ErrorResponce>
            {
                UserRequest = new UserRequest { UserName = userName },
                Data = data
            };

            ErrorResponce responce = await _mediator.Send(request);

            return RedirectToRoute("Projects",new { userName = userName});
        }

        [Route("shapshots")]
        public async Task<IActionResult> ShowShapshots([FromRoute] string userName)
        {
            var request = new HandlerData<UserRequest, IEnumerable<SnapshotData>> { Data = new UserRequest { UserName = userName } };

            IEnumerable<SnapshotData> data = await _mediator.Send(request);

            var table = new NumberedTable(new ObjectTable<SnapshotData>(data));

            return View("SnapshotTable", table);
        }



    }
}
