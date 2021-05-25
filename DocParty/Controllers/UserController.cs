
using DocParty.Extensions;
using DocParty.Filters;
using DocParty.Models;
using DocParty.NotificationHandlers.UserHandlers;
using DocParty.NotificationHandlers.UserHandlers.AddAuthor;
using DocParty.RequestHandlers;
using DocParty.RequestHandlers.AddProject;
using DocParty.RequestHandlers.Profile;
using DocParty.RequestHandlers.Projects;
using DocParty.RequestHandlers.ShowSnapshots;
using DocParty.RequestHandlers.UserHandlers;
using DocParty.RequestHandlers.UserHandlers.DeleteAuthor;
using DocParty.Services.Tables;
using DocParty.ViewModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.Controllers
{
    
    [Authorize]
    [Route("{userName}")]
    [NotFoundPageFilter]
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
        [RestoreModelStateFromTempData]
        public async Task<IActionResult> ShowProjects([FromRoute] UserRoute route)
        {
            await Init(route);

            var request = new HandlerData<User, IEnumerable<ProjectTableData>> { Data = _user};

            IEnumerable<ProjectTableData> data = await _mediator.Send(request);

            string projectLocationTemplate = "/{0}/{1}";
            string[] projectReferences = data
                .Select(rowData => String.Format(projectLocationTemplate, rowData.CreatorName, rowData.ProjectName))
                .ToArray();

            var table = new ReferencedTable(
                new NumberedTable(new ObjectTable<ProjectTableData>(data)),
                new Dictionary<string, string[]>
                {
                    {"ProjectName",projectReferences}
                }
            );  
            return View("Projects", new ProjectsInfo {Table = table,UserName = route.UserName});
        }

        [HttpPost]
        [Route("projects")]
        [SetTempDataModelState]
        public async Task<IActionResult> AddProject([FromRoute] UserRoute route, [FromForm] SnapshotFormData formData)
        {
            if (ModelState.IsValid)
            {
                await Init(route);

                var request = new UserHandlerData<SnapshotFormData, ErrorResponce>
                {
                    User = _user,
                    Data = formData
                };

                ErrorResponce responce = await _mediator.Send(request);

                ModelState.CheckErrors(responce);
            }
            
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

        [Route("authors")]
        [RestoreModelStateFromTempData]
        public async Task<IActionResult> ShowAuthors([FromRoute] UserRoute route)
        {
            await Init(route);

            var request = new HandlerData<User, ILookup<Project,User>>
            {
                Data = _user,
            };

            ILookup<Project,User> authorsByProject = await _mediator.Send(request);

            return View("Authors", authorsByProject);
        }

        [Route("authors")]
        [HttpPost]
        public async Task<IActionResult> AddAuthor([FromForm] AuthorAddingFormData formData, [FromRoute] UserRoute route)
        {
            await Init(route);

            var request = new UserNotificationData<AuthorAddingFormData>
            {
                Data = formData,
                User = _user
            };

            await _mediator.Publish(request);

            return RedirectToAction(nameof(ShowAuthors), route);
        }

        [Route("authors/deletion")]
        [HttpPost]
        public async Task<IActionResult> DeleteAuthor([FromForm] AuthorDeletingFormData formData, [FromRoute] UserRoute route)
        {
            await Init(route);

            var request = new UserHandlerData<AuthorDeletingFormData, Unit>
            {
                Data = formData,
                User = _user
            };

            await _mediator.Send(request);

            return RedirectToAction(nameof(ShowAuthors), route);
        }

    }
}
