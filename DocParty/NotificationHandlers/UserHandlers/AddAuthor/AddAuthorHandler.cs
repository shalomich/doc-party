using DocParty.Models;
using DocParty.Services;
using DocParty.Services.Email;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocParty.NotificationHandlers.UserHandlers.AddAuthor
{
    class AddAuthorHandler : INotificationHandler<UserHandlerData<AuthorAddingFormData>>
    {
        private ApplicationContext Context { get; }
        private AuthorAssignService AssignService { get; }

        public AddAuthorHandler(ApplicationContext context, AuthorAssignService assignService)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            AssignService = assignService ?? throw new ArgumentNullException(nameof(assignService));
        }

        public async Task Handle(UserHandlerData<AuthorAddingFormData> notification, CancellationToken cancellationToken)
        {
            Project project = await Context.Projects
                .FirstAsync(project => project.Name == notification.Data.ProjectName
                        && project.Creator.UserName == notification.User.UserName);

            var unathorizedAuthor = new UnauthorizedAuthor
            {
                Email = notification.Data.Email,
                Project = project
            };

            await AssignService.Assign(unathorizedAuthor);
        }
    }
}
