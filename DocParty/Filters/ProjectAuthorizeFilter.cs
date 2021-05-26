using DocParty.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DocParty.Filters
{
    public class ProjectAuthorizeFilter : IAuthorizationFilter
    {
        private ApplicationContext Context { get; }
        private readonly string[] _roles;

        public ProjectAuthorizeFilter(ApplicationContext context, Role.Value[] roles)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            _roles = roles.Select(role => role.ToString()).ToArray();
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string projectName = (string) context.HttpContext.Request.RouteValues[nameof(projectName)];
            string userName = (string)context.HttpContext.Request.RouteValues[nameof(userName)];
            string currentUserName = context.HttpContext.User.Identity.Name;

            bool isExist = Context.Projects
                .Any(project => project.Name == projectName
                    && project.Creator.UserName == userName);

            if (isExist == false)
            {
                context.Result = new NotFoundResult();
                return;
            }
            
            bool isAccessAllowed = Context.UserRoles
                .Where(userRole => _roles.Contains(userRole.Role.Name)
                    && userRole.User.UserName == currentUserName
                    && userRole.Project.Name == projectName)
                .Any();

            if (isAccessAllowed == false)
                context.Result = new ObjectResult("Forbidden") { StatusCode = 403 };

        }
    }
}
