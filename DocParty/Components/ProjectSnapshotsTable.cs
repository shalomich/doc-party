using DocParty.Models;
using DocParty.Services.Tables;
using DocParty.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.Components
{
    public class ProjectSnapshotsTable : ViewComponent
    {
        public IViewComponentResult Invoke(Project project)
        {
            var snapshotData = project.Snapshots.Select(snapshot => new ProjectSnapshotsTableData
            {
                Name = snapshot.Name,
                AuthorName = snapshot.Author.UserName,
                Description = snapshot.Description
            }).ToArray();

            string projectName = project.Name;
            string creatorName = project.Creator.UserName;

            var table = new ReferencedTable(
                new NumberedTable(new ObjectTable<ProjectSnapshotsTableData>(snapshotData)),
                new Dictionary<string, string>()
                {
                    {"Name",$"/{creatorName}/{projectName}/{{0}}"}
                }
            );

            return View(table);        
        }
    }
}
