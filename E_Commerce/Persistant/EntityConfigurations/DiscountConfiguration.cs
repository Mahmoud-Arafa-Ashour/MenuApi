namespace E_Commerce.Persistant.EntityConfigurations
{
    public class DiscountConfiguration : IEntityTypeConfiguration<Discount>
    {
        public void Configure(EntityTypeBuilder<Discount> builder)
        {
            // ✅ Define Composite Key
            builder.HasKey(x => new { x.CategoryId, x.ItemId });

            builder.Property(x => x.CategoryId).IsRequired();
            builder.Property(x => x.ItemId).IsRequired();
            builder.Property(x => x.NewPrice).IsRequired().HasPrecision(18, 4);
            builder.Property(x => x.OldPrice).IsRequired().HasPrecision(18, 4);

            // ✅ One-to-Many: Category → Discounts
            builder.HasOne(x => x.Category)
                .WithMany(x => x.Discounts)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);

            // ✅ One-to-One: Item ↔ Discount
            builder.HasOne(x => x.Item)
                .WithOne(x => x.Discount)
                .HasForeignKey<Discount>(x => x.ItemId)  // ✅ Use only ItemId as FK
                .OnDelete(DeleteBehavior.NoAction);
        }


    }
}
