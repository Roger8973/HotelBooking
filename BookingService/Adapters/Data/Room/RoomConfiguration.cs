using Entities = Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Room
{
    public class RoomConfiguration : IEntityTypeConfiguration<Entities.Room>
    {
        public void Configure(EntityTypeBuilder<Entities.Room> builder)
        {
            builder.HasKey(x => x.Id);

            builder.OwnsOne(x => x.Price)
                .Property(x => x.Currency);

            builder.OwnsOne(x => x.Price)
                .Property(x => x.Value);
        }
    }
}
