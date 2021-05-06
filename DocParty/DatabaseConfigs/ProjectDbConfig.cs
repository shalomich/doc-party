using DocParty.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.DatabaseConfigs
{
    class ProjectDbConfig : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.ToTable("Projects");

            builder.Property(project => project.Name).IsRequired();
            builder.Property(project => project.Name).HasMaxLength(Project.MaxNameLength);
            builder.Property(project => project.isActive).HasDefaultValue(true);
            builder.HasIndex(project => new {project.Name, project.CreatorId}).IsUnique();
        }
    }
}
