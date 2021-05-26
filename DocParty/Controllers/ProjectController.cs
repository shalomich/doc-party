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
    /// <summary>
    /// MVC controller is responsible for user actions 
    /// for the concrete project. 
    /// </summary>
    /// 
    [Authorize]
    [Route("{userName}/{projectName}")]
    [NotFoundPageFilter]
    public class ProjectController : Controller
    {
        /// <summary>
        /// Service for transfer responsibility from controllers
        /// to request or notification handlers.
        /// </summary>
        private readonly IMediator _mediator;

        /// <summary>
        /// Project of user identified by route.
        /// </summary>
        private Project _project;

        public ProjectController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Show project page. 
        /// </summary>
        /// <param name="route">User name and project name</param>
        /// <returns> Project page.</returns>
        [RestoreModelStateFromTempData]
        [TypeFilter(typeof(ProjectAuthorizeFilter), 
            Arguments = new object[] { new Role.Value[] {Role.Value.Creator,Role.Value.Author} })]
        public async Task<IActionResult> Show([FromRoute] ProjectRoute route)
        {
            //Getting field _project based on route.
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

            // Creation table for project snapshots.
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

        /// <summary>
        /// Initialize field _project.
        /// </summary>
        /// <param name="route">User name and project name</param>
        private async Task Init(ProjectRoute route)
        {
            
            var request = new HandlerData<ProjectRoute, Project>
            {
                Data = route
            };

            _project = await _mediator.Send(request);
        }
        /// <summary>
        /// Adding new project snapshot.
        /// </summary>
        /// <param name="formData">Snapshot name, description and file.</param>
        /// <param name="route">User name and project name.</param>
        /// <returns>Redirect to project page.</returns>
        [HttpPost]
        [SetTempDataModelState]
        [TypeFilter(typeof(ProjectAuthorizeFilter),
            Arguments = new object[] { new Role.Value[] { Role.Value.Creator, Role.Value.Author } })]
        public async Task<IActionResult> AddSnapshot([FromForm] SnapshotFormData formData, [FromRoute] ProjectRoute route)
        {
            if (ModelState.IsValid)
            {
                //Getting field _project based on route.
                await Init(route);

                var request = new ProjectHandlerData<(string UserName, SnapshotFormData FormData), ErrorResponce>
                {
                    Project = _project,
                    Data = (User.Identity.Name, formData)
                };

                // Getting object that contain errors about login user data.
                ErrorResponce responce = await _mediator.Send(request);

                // Checking errors if exist then add them to modelstate.
                ModelState.CheckErrors(responce);
            }

            return RedirectToAction(nameof(Show), route);
        }

        /// <summary>
        /// Deleting project.
        /// </summary>
        /// <param name="route">User name and project name</param>
        /// <returns> Redirect to all user projects page.</returns>
        [HttpPost]
        [Route("deletion")]
        [TypeFilter(typeof(ProjectAuthorizeFilter),
            Arguments = new object[] { new Role.Value[] { Role.Value.Creator} })]
        public async Task<IActionResult> Delete([FromRoute] ProjectRoute route)
        {
            //Getting field _project based on route.
            await Init(route);

            var request = new HandlerData<Project, ErrorResponce>
            {
                Data = _project
            };

            await _mediator.Send(request);

            return RedirectToRoute("Projects", new { userName = route.UserName});
        }

        /// <summary>
        /// Open or close project.
        /// </summary>
        /// <param name="route">User name and project name.</param>
        /// <returns>Redirect to project page.</returns>
        [HttpPost]
        [Route("state")]
        [TypeFilter(typeof(ProjectAuthorizeFilter),
            Arguments = new object[] { new Role.Value[] { Role.Value.Creator} })]
        public async Task<IActionResult> ChangeState([FromRoute] ProjectRoute route)
        {
            //Getting field _project based on route.
            await Init(route);

            var request = new HandlerData<Project, Unit>
            {
                Data = _project,
            };

            await _mediator.Send(request);

            return RedirectToAction(nameof(Show), route);
        }

        /// <summary>
        /// Comment choosed project shapshot.
        /// </summary>
        /// <param name="formData">Snapshot name and text of comment.</param>
        /// <param name="route">User name and project name.</param>
        /// <returns>Redirect to project page.</returns>

        [HttpPost]
        [Route("comment")]
        [SetTempDataModelState]
        [TypeFilter(typeof(ProjectAuthorizeFilter),
            Arguments = new object[] { new Role.Value[] { Role.Value.Creator, Role.Value.Author } })]
        public async Task<IActionResult> Comment([FromForm] CommentFormData formData, [FromRoute] ProjectRoute route)
        {
            if (ModelState.IsValid)
            {
                //Getting field _project based on route.
                await Init(route);

                var request = new ProjectHandlerData<(string UserName, CommentFormData FormData), ErrorResponce>
                {
                    Data = (User.Identity.Name, formData),
                    Project = _project,
                };

                // Getting object that contain errors about login user data.
                ErrorResponce responce = await _mediator.Send(request);

                // Checking errors if exist then add them to modelstate.
                ModelState.CheckErrors(responce);
            }
            
            return RedirectToAction(nameof(Show), route);

        }
    }
}
