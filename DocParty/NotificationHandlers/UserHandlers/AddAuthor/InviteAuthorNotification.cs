using DocParty.Models;
using DocParty.Services.Email;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocParty.NotificationHandlers.UserHandlers.AddAuthor
{
    class InviteAuthorNotification : INotificationHandler<UserNotificationData<AuthorAddingFormData>>
    {
        private const string ProjectLocationTemplate = "https://localhost:44380/{0}/{1}";
        private ApplicationContext Context { get; }
        private EmailSender EmailSender { get; }

        public InviteAuthorNotification(ApplicationContext context, EmailSender emailSender)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            EmailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
        }

        public async Task Handle(UserNotificationData<AuthorAddingFormData> notification, CancellationToken cancellationToken)
        {
            /*User user = await Context.Users
                .Include(user => user.ProjectRoles)
                .FirstOrDefaultAsync(user => user.Email == notification.Data.Email);

            if (user == null)
            {
                string projectLocation = String.Format(notification.User.UserName, notification.Data.ProjectName);
                var builder = new ProjectInviteBuilder("");
                var creator = new EmailMessageCreator(builder);
                await EmailSender.SendEmailAsync(notification.Data.Email, creator.Create());
            }*/
        }
    }
}
