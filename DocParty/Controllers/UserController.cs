
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
    /// <summary>
    /// MVC controller is responsible for authorized user actions 
    /// outside of the concrete project. 
    /// </summary>

    [Authorize]
    [Route("{userName}")]
    [NotFoundPageFilter]
    public class UserController : Controller
    {
        /// <summary>
        /// Service for transfer responsibility from controllers
        /// to request or notification handlers.
        /// </summary>
        private readonly IMediator _mediator;

        /// <summary>
        /// Object of user identified by route.
        /// </summary>
        private User _user;

        public UserController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
  
        /// <summary>
        /// Show user profile with his project statistics.
        /// </summary>
        /// <param name="route">Username</param>
        /// <returns> User profile page. </returns>
        public async Task<IActionResult> ShowProfile([FromRoute] UserRoute route)
        {
            //Getting field _user based on route.
            await Init(route);

            var request = new HandlerData<User, UserStatistics> { Data = _user };
            
            UserStatistics statistics = await _mediator.Send(request);

            // Transformation object of user statistics to dictionary
            // with key as property name and value as property value.
            Dictionary<string,int> data = statistics.GetType().GetProperties()
                .Select(property => KeyValuePair.Create(property.Name, property.GetValue(statistics)))
                .ToDictionary(pair => pair.Key, pair => (int) pair.Value);
            
            return View("Profile", data);
        }

        /// <summary>
        /// Initialize filed _user.
        /// </summary>
        /// <param name="route"> Username</param>
        private async Task Init(UserRoute route)
        {
            var request = new HandlerData<UserRoute, User>
            {
                Data = route
            };

            _user = await _mediator.Send(request);
        }

        /// <summary>
        /// Show all user projects.
        /// </summary>
        /// <param name="route"> Username. </param>
        /// <returns> User project</returns>
        [Route("projects")]
        [RestoreModelStateFromTempData]
        public async Task<IActionResult> ShowProjects([FromRoute] UserRoute route)
        {
            // Getting field _user based on route.
            await Init(route);

            var request = new HandlerData<User, IEnumerable<ProjectTableData>> { Data = _user};

            IEnumerable<ProjectTableData> data = await _mediator.Send(request);

            string projectLocationTemplate = "/{0}/{1}";
            
            // Getting references to user projects.
            string[] projectReferences = data
                .Select(rowData => String.Format(projectLocationTemplate, rowData.CreatorName, rowData.ProjectName))
                .ToArray();

            // Creation all user project table.
            var table = new ReferencedTable(
                new NumberedTable(new ObjectTable<ProjectTableData>(data)),
                new Dictionary<string, string[]>
                {
                    {"ProjectName",projectReferences}
                }
            );  
            return View("Projects", new ProjectsInfo {Table = table,UserName = route.UserName});
        }

        /// <summary>
        /// Add new project for user.
        /// </summary>
        /// <param name="route"> Username.</param>
        /// <param name="formData"> Project name, description and file</param>
        /// <returns>
        /// Redirect to projects page
        /// </returns>
        [HttpPost]
        [Route("projects")]
        [SetTempDataModelState]
        public async Task<IActionResult> AddProject([FromRoute] UserRoute route, [FromForm] SnapshotFormData formData)
        {
            if (ModelState.IsValid)
            {
                // Getting field _user based on route.
                await Init(route);

                var request = new UserHandlerData<SnapshotFormData, ErrorResponce>
                {
                    User = _user,
                    Data = formData
                };

                // Getting object that contain errors about login user data.
                ErrorResponce responce = await _mediator.Send(request);

                // Checking errors if exist then add them to modelstate.
                ModelState.CheckErrors(responce);
            }
            
            return RedirectToRoute("Projects", route);
        }

        /// <summary>
        /// Show data of all user project snapshots.
        /// </summary>
        /// <param name="route">Username.</param>
        /// <returns> All user snapshots page.</returns>
        [Route("shapshots")]
        public async Task<IActionResult> ShowShapshots([FromRoute] UserRoute route)
        {
            // Getting field _user based on route.
            await Init(route);

            var request = new HandlerData<User, IEnumerable<SnapshotTableData>> { Data = _user};

            IEnumerable<SnapshotTableData> data = await _mediator.Send(request);

            var table = new NumberedTable(new ObjectTable<SnapshotTableData>(data));

            return View("UserSnapshotsTable", table);
        }

        /// <summary>
        /// Open page for project authors view and redaction.
        /// </summary>
        /// <param name="route">Username.</param>
        /// <returns> Author redaction panel.</returns>
        [Route("authors")]
        [RestoreModelStateFromTempData]
        public async Task<IActionResult> ShowAuthors([FromRoute] UserRoute route)
        {
            //Getting field _user based on route.
            await Init(route);

            var request = new HandlerData<User, ILookup<Project,User>>
            {
                Data = _user,
            };

            ILookup<Project,User> authorsByProject = await _mediator.Send(request);

            return View("Authors", authorsByProject);
        }

        /// <summary>
        /// Adding author.
        /// </summary>
        /// <param name="formData">Author email and project name.</param>
        /// <param name="route">Username.</param>
        /// <returns>Redirect to author redaction panel.</returns>
        [Route("authors")]
        [HttpPost]
        public async Task<IActionResult> AddAuthor([FromForm] AuthorAddingFormData formData, [FromRoute] UserRoute route)
        {
            //Getting field _user based on route.
            await Init(route);

            var request = new UserHandlerData<AuthorAddingFormData>
            {
                Data = formData,
                User = _user
            };

            // Sending request to all notification handlers that process author adding. 
            await _mediator.Publish(request);

            return RedirectToAction(nameof(ShowAuthors), route);
        }

        /// <summary>
        /// Deleting author.
        /// </summary>
        /// <param name="formData"> Author name and project name.</param>
        /// <param name="route">Username.</param>
        /// <returns></returns>

        [Route("authors/deletion")]
        [HttpPost]
        public async Task<IActionResult> DeleteAuthor([FromForm] AuthorDeletingFormData formData, [FromRoute] UserRoute route)
        {
            //Getting field _user based on route.
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
