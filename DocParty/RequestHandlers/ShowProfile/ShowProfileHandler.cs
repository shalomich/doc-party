using DocParty.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocParty.RequestHandlers.Profile
{
    class ShowProfileHandler : IRequestHandler<HandlerData<UserRequest,UserProfile>, UserProfile>
    {
        private readonly IReadOnlyDictionary<string, Func<User, int>> MetricFunctions = new Dictionary<string, Func<User, int>>()
        {
            {"All project's count", user => user.Projects.Count()},
            {"All project snapshots'count", user => user.Projects.Select(project => project.Snapshots.Count()).Sum()},
            {"ProjectRoles's count in creator role", user => user.Projects.Where(project => project.CreatorId == user.Id).Count() },
            {"ProjectRoles's count in author role", user => user.Projects.Where(project => project.CreatorId != user.Id).Count() },
            {"Closed project' count", user => user.Projects.Where(project => project.isActive == false).Count() }
        };
        private ApplicationContext Context { get; }

        public ShowProfileHandler(ApplicationContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        

        public async Task<UserProfile> Handle(HandlerData<UserRequest, UserProfile> request, CancellationToken cancellationToken)
        {
            User user = await Context.Users
                                    .Include(user => user.Projects)
                                    .ThenInclude(project => project.Snapshots)
                                    .FirstOrDefaultAsync(user => user.UserName == request.Data.UserName);

            return new UserProfile
            {
                Statistics = MetricFunctions.ToDictionary(metric => metric.Key, metric => metric.Value(user))
            };
        }
    }
}
