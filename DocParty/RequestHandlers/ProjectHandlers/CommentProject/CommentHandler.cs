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
    class CommentHandler : IRequestHandler<UserHandlerData<CommentFormData, ErrorResponce>, ErrorResponce>
    {
        private ApplicationContext Context { get; }

        public CommentHandler(ApplicationContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<ErrorResponce> Handle(UserHandlerData<CommentFormData, ErrorResponce> request, CancellationToken cancellationToken)
        {
            var errors = new List<string>();

            var author = await Context.Users
                .FirstAsync(user => user.UserName == request.UserRequest.UserName);

            var snapshot = await Context.ProjectShapshots
                .FirstAsync(snapshot => snapshot.Author.UserName == request.UserRequest.UserName
                    && snapshot.Project.Name == ((ProjectRequest)request.UserRequest).ProjectName
                    && snapshot.Name == request.Data.SnapshotName);

            var comment = new Comment(snapshot, author);

            try
            {
                comment.Text = request.Data.Text;
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
