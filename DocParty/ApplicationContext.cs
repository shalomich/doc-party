using DocParty.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocParty.DatabaseConfigs;

namespace DocParty
{
    public class ApplicationContext : IdentityDbContext<User, Role, int,
        IdentityUserClaim<int>, UserProjectRole, IdentityUserLogin<int>,
        IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DbSet<Project> Projects { set; get; }
        public DbSet<ProjectSnapshot> ProjectShapshots { set; get; }
        public DbSet<Comment> Comments { set; get; }
        public DbSet<UnauthorizedAuthor> UnauthorizedAuthors { set; get; }


        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ProjectDbConfig());
            modelBuilder.ApplyConfiguration(new ProjectSnapshotDbConfig());
            modelBuilder.ApplyConfiguration(new CommentDbConfig());
            modelBuilder.ApplyConfiguration(new UnauthorizedAuthorDbConfig());

            modelBuilder.Entity<User>(
                user => user
                    .HasMany(user => user.ProjectRoles)
                    .WithOne(projetRole => projetRole.User)
                    .HasForeignKey(projectRole => projectRole.UserId)
                    .IsRequired()
            );

            modelBuilder.Entity<Role>( 
                role => role
                    .HasMany(role => role.ProjectRoles)
                    .WithOne(projectRole => projectRole.Role)
                    .HasForeignKey(projectRole => projectRole.RoleId)
                    .IsRequired()
            );

            modelBuilder.Entity<Role>().HasData(
                new Role[]
                {
                    new Role {Id = 1, Name = Role.Value.Creator.ToString()},
                    new Role {Id = 2, Name = Role.Value.Author.ToString()}
                }
            );

            modelBuilder.Entity<UserProjectRole>()
                .HasKey(userProjectRole => new
                {
                    userProjectRole.UserId,
                    userProjectRole.RoleId,
                    userProjectRole.ProjectId
                });
        }
    }
}
