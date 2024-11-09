using DataAccess.Entities.Interfaces;
using Library.Entities.Interfaces;
using Library.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Context.EntitiesConfigurations
{
    public class MessageConfiguration<T> : IEntityTypeConfiguration<T> where T : class, IMessageEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.SendingUserId).IsRequired();

            builder.Property(e => e.AcceptingUserId).IsRequired();

            builder.Property(e => e.Status)
                .IsRequired()
                .HasDefaultValue(MessageStatus.Pending);

            builder.Property(e => e.IssuedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(e => e.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(e => e.ExpiresAt)
                .IsRequired();
        }
    }
}
