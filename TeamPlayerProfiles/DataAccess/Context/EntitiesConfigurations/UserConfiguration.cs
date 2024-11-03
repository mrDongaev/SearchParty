using DataAccess.Entities.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;

namespace DataAccess.Context.EntitiesConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public virtual void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(e => e.Id)
                .IsRequired();

            builder.Property(e => e.Mmr)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(e => e.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}
