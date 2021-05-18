using DocParty.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DocParty.RequestHandlers.UserHandlers;

namespace DocParty.RequestHandlers.AddProject
{
    class AddProjectHandler : IRequestHandler<UserHandlerData<SnapshotFormData,ErrorResponce>, ErrorResponce>
    {
        private const string InvalidProjectName = "This project name is belongs to your other project";
        private ApplicationContext Context { get; }

        public AddProjectHandler(ApplicationContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }
        
        public async Task<ErrorResponce> Handle(UserHandlerData<SnapshotFormData, ErrorResponce> request, CancellationToken cancellationToken)
        {
            var errors = new List<string>();

            bool isExist = Context.Projects
                .Where(project => project.Creator.UserName == request.User.UserName)
                .Any(project => project.Name == request.Data.Name);

            if (isExist)
            {
                errors.Add(InvalidProjectName);
                return new ErrorResponce(errors);
            }
            
            var project = new Project(request.Data.Name, request.Data.Description, request.User);

            await Context.Projects.AddAsync(project);

            Context.SaveChanges();

            return new ErrorResponce(errors);

        }
    }
}
