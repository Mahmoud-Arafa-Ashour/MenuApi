namespace E_Commerce.Persistant.EntityConfigurations
{
    public class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.Property(x => x.Price)
                .HasPrecision(18, 4);
            builder.HasIndex(x=>x.Name)
                .IsUnique();
        }
    }
}
