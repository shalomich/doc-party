using DocParty.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.Components
{
    public class Header : ViewComponent
    {
        public const string UserProfileLocation = "/{0}";
        public const string UserProjectsLocation = "/{0}/projects";
        public const string UserProjectSnapshotsLocation = "/{0}/shapshots";
        public const string UserProjectAuthorsLocation = "/{0}/authors";
        public IViewComponentResult Invoke()
        {
            string userName = HttpContext.User.Identity.Name;

            var info = new HeaderInfo
            {
                UserName = userName,
                UserProfileLocation = String.Format(UserProfileLocation, userName),
                ProjectsLocation = String.Format(UserProjectsLocation, userName),
                ProjectSnapshotsLocation = String.Format(UserProjectSnapshotsLocation, userName),
                AuthorsLocation = String.Format(UserProjectAuthorsLocation, userName)
            };

            return View(info);
        }
    }
}
