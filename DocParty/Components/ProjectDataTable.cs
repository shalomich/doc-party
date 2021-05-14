
using DocParty.RequestHandlers.Projects;
using DocParty.Services;
using DocParty.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.Components
{
    public class ProjectDataTable : ViewComponent
    {
        private const string ProjectLocation = "/{0}/{1}";
        
        private readonly Table<ProjectData> _table;

        public ProjectDataTable(Table<ProjectData> table)
        {
            _table = table ?? throw new ArgumentNullException(nameof(table));
        }

        public IViewComponentResult Invoke(ProjectsInfo info)
        {
            _table.Fill(info.Data);

            Dictionary<string,string> projectsLocations = info.Data.ToDictionary
            (
                projectData => projectData.ProjectName,
                projectData => String.Format(ProjectLocation, info.UserName, projectData.ProjectName)
            );

            return View(new ProjectDataTableInfo { Table = _table, ProjectsLocations = projectsLocations});
        }


    }
}
