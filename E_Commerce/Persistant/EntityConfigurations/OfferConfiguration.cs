
namespace E_Commerce.Persistant.EntityConfigurations
{
    public class OfferConfiguration : IEntityTypeConfiguration<Offer>
    {
        public void Configure(EntityTypeBuilder<Offer> builder)
        {
            builder.HasKey(x=>x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
            builder.Property(x=>x.IsActive).HasDefaultValue(true);
            builder.Property(x => x.Price).HasColumnType("decimal(18,2)");
        }
    }
}
