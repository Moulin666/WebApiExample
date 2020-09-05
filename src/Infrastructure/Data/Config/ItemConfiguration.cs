using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class ItemConfiguration : IEntityTypeConfiguration<ShopItem>
    {
        public void Configure(EntityTypeBuilder<ShopItem> builder)
        {
            builder.ToTable("ShopItems");

            builder.Property(i => i.Id)
                .IsRequired();

            builder.Property(i => i.Name)
                .IsRequired(true)
                .HasMaxLength(50);

            builder.Property(i => i.Price)
                .IsRequired(true)
                .HasColumnType("decimal(18,2)");

            builder.Property(i => i.PictureUri)
                .IsRequired(false);
        }
    }
}
