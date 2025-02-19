namespace E_Commerce.Persistant.EntityConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(x => x.Adress)
                .IsRequired().HasMaxLength(100);
            builder.Property(x => x.ResturnatName)
                .IsRequired().HasMaxLength(100);
            builder.OwnsMany(x => x.RefreshTokens)
                .ToTable("RefreshTokens")
                .WithOwner()
                .HasForeignKey("UserId");
        }
    }

}
