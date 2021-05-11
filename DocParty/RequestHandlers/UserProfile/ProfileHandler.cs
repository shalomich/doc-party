using DocParty.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocParty.RequestHandlers.UserProfile
{
    class ProfileHandler : IRequestHandler<UserQuery, UserStatistics>
    {
        private readonly IReadOnlyDictionary<string, Func<User, int>> MetricFunctions = new Dictionary<string, Func<User, int>>()
        {
            {"All project's count", user => user.Projects.Count()},
            {"All project snapshots'count", user => user.Projects.Select(project => project.Snapshots.Count()).Sum()},
            {"Projects's count in creator role", user => user.Projects.Where(project => project.CreatorId == user.Id).Count() },
            {"Projects's count in author role", user => user.Projects.Where(project => project.CreatorId != user.Id).Count() },
            {"Closed project' count", user => user.Projects.Where(project => project.isActive == false).Count() }
        };
        private ApplicationContext Context { get; }

        public ProfileHandler(ApplicationContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<UserStatistics> Handle(UserQuery request, CancellationToken cancellationToken)
        {
            User user = await Context.Users
                                    .Include(user => user.Projects)
                                    .ThenInclude(project => project.Snapshots)
                                    .FirstOrDefaultAsync(user => user.UserName == request.UserName);

            return new UserStatistics
            {
                Data = MetricFunctions.Select(metric => (metric.Key, metric.Value(user))).ToList()
            };
        }
    }
}
