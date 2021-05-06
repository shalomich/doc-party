using DocParty.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.DatabaseConfigs
{
    class ProjectSnapshotDbConfig : IEntityTypeConfiguration<ProjectSnapshot>
    {
        public void Configure(EntityTypeBuilder<ProjectSnapshot> builder)
        {
            builder.ToTable("ProjectSnapshots");

            builder.Property(snapshot => snapshot.Name).IsRequired();
            builder.Property(snapshot => snapshot.Name).HasMaxLength(ProjectSnapshot.MaxNameLength);
            builder.Property(snapshot => snapshot.Description).IsRequired();
            builder.HasIndex(snapshot => new { snapshot.Name, snapshot.ProjectId });
        }
    }
}
