using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using DocParty.Filters;
using DocParty.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;

namespace DocParty.Tests
{
    public class ProjectAuthorizeFilterTests
    {
        private readonly ApplicationContext _dbContext; 
        public ProjectAuthorizeFilterTests()
        {
            _dbContext = new ApplicationContext(new DbContextOptions<ApplicationContext>());

            var user1 = new User { UserName = "user1" };
            var user2 = new User { UserName = "user2" };

            var project1 = new Project("project1","","",user1);
            var project2 = new Project ("project2", "","", user1);
            var project3 = new Project ("project3","","",user2);

            var userRole1 = new UserProjectRole
            {
                User = user1,
                Project = project1,
                Role = new Role { Name = Role.Value.Creator.ToString()}
            };

            var userRole2 = new UserProjectRole
            {
                User = user1,
                Project = project2,
                Role = new Role { Name = Role.Value.Creator.ToString() }
            };

            var userRole3 = new UserProjectRole
            {
                User = user2,
                Project = project3,
                Role = new Role { Name = Role.Value.Creator.ToString() }
            };

            var userRole4 = new UserProjectRole
            {
                User = user2,
                Project = project2,
                Role = new Role { Name = Role.Value.Author.ToString() }
            };

            _dbContext.Projects = GetQueryableMockDbSet(new Project[] 
                { 
                    project1,project2,project3
                });

            _dbContext.UserRoles = GetQueryableMockDbSet(new UserProjectRole[] 
            {
                userRole1,userRole2,userRole3,userRole4
            });
        }

        [Theory]
        [MemberData(nameof(GetReceiveProjectAcessData))]
        public void ReceiveProjectAccess(string userName, string projectName, string currentUserName, Role.Value[] roles)
        {
            var result = TryGetProjectAccess(userName, projectName, currentUserName, roles);

            Assert.Null(result);
        }

        public static IEnumerable<object[]> GetReceiveProjectAcessData()
        {
            yield return new object[]
            {
                "user1","project1","user1", new Role.Value[]{ Role.Value.Creator, Role.Value.Author},
            };
            yield return new object[]
            {
                "user2","project3","user2", new Role.Value[]{ Role.Value.Creator},
            };
            yield return new object[]
            {
                "user1","project2","user2", new Role.Value[]{ Role.Value.Creator, Role.Value.Author}
            };
        }

        [Theory]
        [MemberData(nameof(GetDenyProjectAcessData))]
       
        public void DenyProjectAcess(string userName, string projectName, string currentUserName, Role.Value[] roles)
        {
            var result = TryGetProjectAccess(userName, projectName, currentUserName, roles);

            Assert.IsType<ObjectResult>(result);
        }

        public static IEnumerable<object[]> GetDenyProjectAcessData()
        {
            yield return new object[]
            {
                "user1","project1","user2", new Role.Value[]{ Role.Value.Creator, Role.Value.Author},
            };
            yield return new object[]
            {
                "user2","project3","user1", new Role.Value[]{ Role.Value.Creator, Role.Value.Author},
            };
            yield return new object[]
            {
                "user1","project2","user2", new Role.Value[]{ Role.Value.Creator}
            };
        }

        [Theory]
        [MemberData(nameof(GetNotFoundProjectData))]

        public void NotFoundProject(string userName, string projectName, string currentUserName, Role.Value[] roles)
        {
            var result = TryGetProjectAccess(userName, projectName, currentUserName, roles);

            Assert.IsType<NotFoundResult>(result);
        }

        public static IEnumerable<object[]> GetNotFoundProjectData()
        {
            yield return new object[]
            {
                "user1","project3","user1", new Role.Value[]{ Role.Value.Creator, Role.Value.Author},
            };
            yield return new object[]
            {
                "user2","project1","user2", new Role.Value[]{ Role.Value.Creator, Role.Value.Author},
            };
            yield return new object[]
            {
                "user1","qwerty","user2", new Role.Value[]{ Role.Value.Creator}
            };
        }

        private IActionResult TryGetProjectAccess(string userName, string projectName, string currentUserName, Role.Value[] roles)
        {
            var routeValues = new RouteValueDictionary() {
                {nameof(userName),userName},
                {nameof(projectName),projectName}
            };

            var httpContext = Mock.Of<HttpContext>(context =>
                context.User.Identity.Name == currentUserName
                && context.Request.RouteValues == routeValues);

            var authContext = new AuthorizationFilterContext(
                new ActionContext(
                    httpContext,
                    new RouteData(routeValues),
                    new ActionDescriptor()),
                new List<IFilterMetadata>());

            var filter = new ProjectAuthorizeFilter(_dbContext, roles);
            filter.OnAuthorization(authContext);
            
            return authContext.Result;
        }
        



        private static DbSet<T> GetQueryableMockDbSet<T>(params T[] sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();

            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

            return dbSet.Object;
        }
    }
}
