using DocParty.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.Components
{
    public class AuthorAddingForm : ViewComponent
    {
        public IViewComponentResult Invoke(ILookup<Project,User> authorsByProject)
        {
            string[] projectNames = authorsByProject
                .Select(authorByProject => authorByProject.Key.Name).ToArray();
            var select = new SelectList(projectNames);

            return View(select);
        }
    }
}
