
using DocParty.Models;
using DocParty.RequestHandlers;
using DocParty.RequestHandlers.AddProject;
using DocParty.RequestHandlers.Profile;
using DocParty.RequestHandlers.Projects;
using DocParty.RequestHandlers.ShowSnapshots;
using DocParty.RequestHandlers.UserHandlers;
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

        private User _user;

        public UserController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
  
        public async Task<IActionResult> ShowProfile([FromRoute] UserRoute route)
        {
            await Init(route);

            var request = new HandlerData<User, UserStatistics> { Data = _user };
            
            UserStatistics statistics = await _mediator.Send(request);

            Dictionary<string,int> data = statistics.GetType().GetProperties()
                .Select(property => KeyValuePair.Create(property.Name, property.GetValue(statistics)))
                .ToDictionary(pair => pair.Key, pair => (int) pair.Value);
            
            return View("Profile", data);
        }

        private async Task Init(UserRoute route)
        {

            var request = new HandlerData<UserRoute, User>
            {
                Data = route
            };

            _user = await _mediator.Send(request);
        }

        [Route("projects")]
        public async Task<IActionResult> ShowProjects([FromRoute] UserRoute route)
        {
            await Init(route);

            var request = new HandlerData<User, IEnumerable<ProjectTableData>> { Data = _user};

            IEnumerable<ProjectTableData> data = await _mediator.Send(request);

            var columnReferenceTemplates = new Dictionary<string, string>
            {
                {"ProjectName", $"/{route.UserName}/{{0}}" }
            };

            var table = new ReferencedTable(
                new NumberedTable(new ObjectTable<ProjectTableData>(data)),
                columnReferenceTemplates
            );  
            return View("Projects", new ProjectsInfo {Table = table,UserName = route.UserName});
        }

        [HttpPost]
        [Route("projects")]
        public async Task<IActionResult> AddProject([FromRoute] UserRoute route, [FromForm] SnapshotFormData formData)
        {
            await Init(route);

            var request = new UserHandlerData<SnapshotFormData, ErrorResponce>
            {
                User = _user,
                Data = formData
            };

            ErrorResponce responce = await _mediator.Send(request);

            return RedirectToRoute("Projects", route);
        }

        [Route("shapshots")]
        public async Task<IActionResult> ShowShapshots([FromRoute] UserRoute route)
        {
            await Init(route);

            var request = new HandlerData<User, IEnumerable<SnapshotTableData>> { Data = _user};

            IEnumerable<SnapshotTableData> data = await _mediator.Send(request);

            var table = new NumberedTable(new ObjectTable<SnapshotTableData>(data));

            return View("UserSnapshotsTable", table);
        }



    }
}
