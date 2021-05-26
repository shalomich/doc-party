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
    public class CommentHandler : IRequestHandler<ProjectHandlerData<(string UserName,CommentFormData FormData), ErrorResponce>, ErrorResponce>
    {
        private ApplicationContext Context { get; }

        public CommentHandler(ApplicationContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<ErrorResponce> Handle(ProjectHandlerData<(string UserName, CommentFormData FormData), ErrorResponce> request, CancellationToken cancellationToken)
        {
            var errors = new List<string>();

            var author = Context.Users
                .First(user => user.UserName == request.Data.UserName);

            var snapshot = Context.ProjectShapshots
                .First(snapshot => snapshot.Project.Id == request.Project.Id
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

            Context.Comments.Add(comment);
            Context.SaveChanges();

            return new ErrorResponce(errors);
        }
    }
}
