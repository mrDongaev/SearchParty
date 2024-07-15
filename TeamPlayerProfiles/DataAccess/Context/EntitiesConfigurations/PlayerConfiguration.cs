using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DataAccess.Entities;

namespace DataAccess.Context.Configurations
{
    public class PlayerConfiguration: ProfileConfiguration<Player>
    {
        public override void Configure(EntityTypeBuilder<Player> builder)
        {
            builder.Property(e => e.Position)
                .IsRequired();

            base.Configure(builder);
        }
    }
}
