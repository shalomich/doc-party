using DocParty.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DocParty.RequestHandlers.UserHandlers;
using Microsoft.AspNetCore.Identity;
using DocParty.Services.Repositories;
using System.IO;
using DocParty.RequestHandlers.SnapshotHandlers.ShowFile;

namespace DocParty.RequestHandlers.AddProject
{
    class AddProjectHandler : IRequestHandler<UserHandlerData<SnapshotFormData,ErrorResponce>, ErrorResponce>
    {
        private const string InvalidProjectName = "This project name is belongs to your other project";
        private ApplicationContext Context { get; }
        private IRepository<byte[],string> Repository { get; }

        public AddProjectHandler(ApplicationContext context, RoleManager<Role> roleManager, IRepository<byte[], string> repository)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// Creation new project in database and saving project file in repository
        /// </summary>
        /// <param name="request">User, project name, desciption and file.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// If project with same name is exist return error
        /// else return empty error responce.
        /// </returns>
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
            
            
            Role creatorRole = await Context.Roles.FirstAsync(role => role.Name == Role.Value.Creator.ToString());
             
            var user = await Context.Users
                .Include(user => user.ProjectRoles)
                .FirstAsync(user => user.UserName == request.User.UserName);

            var project = new Project(request.Data.Name, request.Data.Description, request.Data.File.ContentType, user);
            
            // Adding for user role creator for current project.
            user.ProjectRoles = user.ProjectRoles.ToList();
            ((List<UserProjectRole>)user.ProjectRoles).Add(new UserProjectRole
            {
                Project = project,
                Role = creatorRole
            });

            Context.Users.Update(user);
            await Context.SaveChangesAsync();

            // Convert file to byte array.
            byte[] fileBytes;
            using (var stream = new MemoryStream())
            {
                request.Data.File.CopyTo(stream);
                fileBytes = stream.ToArray();
            };

            // Make file name base on his name and content type.
            // For example, GetFileName("1", "text/plain") = "1.txt"
            string fileName = FileData.GetFileName(project.Snapshots.First().Id.ToString(), project.FileContentType);

            Repository.Create(fileName, fileBytes);
        
            return new ErrorResponce(errors);
        }
    }
}
