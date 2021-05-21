using DocParty.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DocParty.RequestHandlers.CommentProject;
using Microsoft.EntityFrameworkCore;

namespace DocParty.RequestHandlers.ProjectHandlers.CommentProject
{
    class CommentHandler : IRequestHandler<ProjectHandlerData<(string UserName,CommentFormData FormData), ErrorResponce>, ErrorResponce>
    {
        private ApplicationContext Context { get; }

        public CommentHandler(ApplicationContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<ErrorResponce> Handle(ProjectHandlerData<(string UserName, CommentFormData FormData), ErrorResponce> request, CancellationToken cancellationToken)
        {
            var errors = new List<string>();

            var author = await Context.Users
                .FirstAsync(user => user.UserName == request.Data.UserName);

            var snapshot = await Context.ProjectShapshots
                .FirstAsync(snapshot => snapshot.Project.Creator.UserName == request.Project.Creator.UserName
                    && snapshot.Project.Name == request.Project.Name
                    && snapshot.Name == request.Data.FormData.SnapshotName);

            var comment = new Comment(snapshot, author);

            try
            {
                comment.Text = request.Data.FormData.Text;
            }
            catch (Exception exception)
            {
                errors.Add(exception.Message);
                return new ErrorResponce(errors);
            }

            await Context.Comments.AddAsync(comment);
            await Context.SaveChangesAsync();

            return new ErrorResponce(errors);
        }
    }
}
