using DocParty.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using DocParty.RequestHandlers.ProjectHandlers.CommentProject;
using DocParty.RequestHandlers.CommentProject;
using DocParty.RequestHandlers.ProjectHandlers;
using System.Threading.Tasks;
using DocParty.RequestHandlers;
using System.Threading;

namespace DocParty.Tests
{
    public class CommentHandlerTests
    {
        private readonly ApplicationContext _dbContext;
        public CommentHandlerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("comments")
                .Options;

            _dbContext = new ApplicationContext(options);

            var user = new User { UserName = "user" };
            var project = new Project("project", "", "", user);

            project.Snapshots.Add(new ProjectSnapshot { Name = "snapshot1" });
            project.Snapshots.Add(new ProjectSnapshot { Name = "snapshot2" });


            _dbContext.Projects.Add(project);
            _dbContext.SaveChanges();
        }

        [Theory]
        [MemberData(nameof(GetCorrectCommentData))]
        public async Task CorrectComment(CommentFormData formData)
        {
            var responce = await Comment(formData);

            Assert.Empty(responce.Errors);
        }

        public static IEnumerable<object[]> GetCorrectCommentData()
        {
            yield return new object[]
            {
                new CommentFormData {SnapshotName = "project", Text = "text1"}
            };
            yield return new object[]
            {
                new CommentFormData {SnapshotName = "snapshot1", Text = "text2"}
            };
            yield return new object[]
            {
                new CommentFormData {SnapshotName = "snapshot2", Text = "text3"}
            };
        }

        [Theory]
        [MemberData(nameof(GetCommentWithBigTextData))]
        public async Task CommentWithBigText(CommentFormData formData)
        {
            var responce = await Comment(formData);

            Assert.Single(responce.Errors);
        }

        public static IEnumerable<object[]> GetCommentWithBigTextData()
        {
            Func<int,string> makeBigString = (number) => 
                Enumerable.Repeat("a", number).Aggregate((str1,str2)=> str1 + str2);

            yield return new object[]
            {
                new CommentFormData {SnapshotName = "project", Text = makeBigString(101)}
            };
            yield return new object[]
            {
                new CommentFormData {SnapshotName = "snapshot1", Text = makeBigString(150)}
            };
            yield return new object[]
            {
                new CommentFormData {SnapshotName = "snapshot2", Text = makeBigString(200)}
            };
        }

        private async Task<ErrorResponce> Comment(CommentFormData formData)
        {
            var handler = new CommentHandler(_dbContext);
            return await handler.Handle(
                new ProjectHandlerData<(string userName, CommentFormData formData), ErrorResponce>
                {
                    Data = ("user", formData),
                    Project = _dbContext.Projects.First(project => project.Name == "project")
                }, new CancellationToken());
        }


    }
}
