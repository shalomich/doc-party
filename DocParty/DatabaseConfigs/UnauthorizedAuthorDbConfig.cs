using DocParty.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.DatabaseConfigs
{
    class UnauthorizedAuthorDbConfig : IEntityTypeConfiguration<UnauthorizedAuthor>
    {
        public void Configure(EntityTypeBuilder<UnauthorizedAuthor> builder)
        {
            builder.ToTable("UnauthorizedAuthors");

            builder.Property(author => author.Email).IsRequired();
        }
    }
}
