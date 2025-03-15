
namespace E_Commerce.Persistant.EntityConfigurations
{
    public class OfferItemConfiguration : IEntityTypeConfiguration<OfferItem>
    {
        public void Configure(EntityTypeBuilder<OfferItem> builder)
        {
            builder.HasKey(x => new { x.OfferId, x.CategoryId, x.ItemId });
            builder.HasOne(x => x.Offer).WithMany(x => x.OfferItems).HasForeignKey(x => x.OfferId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.Item).WithMany(x => x.OfferItems).HasForeignKey(x => x.ItemId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.Category).WithMany(x => x.OfferItems).HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
