using DocParty.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocParty.RequestHandlers.UserHandlers.ShowAuthors
{
    class ShowAuthorsHandler : IRequestHandler<HandlerData<User, ILookup<Project,User>>, ILookup<Project, User>>
    {
        private ApplicationContext Context { get; }

        public ShowAuthorsHandler(ApplicationContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<ILookup<Project, User>> Handle(HandlerData<User, ILookup<Project, User>> request, CancellationToken cancellationToken)
        {
            User creator = request.Data;

            return Context.Projects
                .Where(project => project.Creator.UserName == creator.UserName)
                .SelectMany(project => project.AuthorRoles
                    .Select(authorRole => new 
                        { Project = project, 
                          Author = authorRole.User.UserName == creator.UserName ? null : authorRole.User
                        }))
                .ToLookup(projectAuthor => projectAuthor.Project,
                    projectAuthor => projectAuthor.Author);
        }
    }
}
