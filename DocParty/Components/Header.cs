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
        public const string UserProjectsUrl = "/{0}/projects";
        public const string UserProjectSnapshotsUrl = "/{0}/shapshots";
        
        public IViewComponentResult Invoke()
        {
            string userName = HttpContext.User.Identity.Name;

            var info = new HeaderInfo
            {
                UserName = userName,
                ProjectsUrl = String.Format(UserProjectsUrl, userName),
                ProjectSnapshotsUrl = String.Format(UserProjectSnapshotsUrl, userName)
            };
            
            return View(info);
        }
    }
}
