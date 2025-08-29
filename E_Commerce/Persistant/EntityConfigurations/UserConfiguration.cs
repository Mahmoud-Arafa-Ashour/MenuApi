namespace E_Commerce.Persistant.EntityConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Adress).IsRequired().HasMaxLength(100);
        builder.Property(x => x.ResturnatName).IsRequired().HasMaxLength(100);
        builder.OwnsMany(x => x.RefreshTokens)
            .ToTable("RefreshTokens")
            .WithOwner()
            .HasForeignKey("UserId");
        builder.HasData(new ApplicationUser
        {
            Id = DefaultUser.AdminId,
            Name = "Digital Menu",
            Adress = "Digital Menu Street",
            ResturnatName = "Digital Menu Restaurant",
            UserName = DefaultUser.AdminEmail,
            NormalizedUserName = DefaultUser.AdminEmail.ToUpper(),
            Email = DefaultUser.AdminEmail,
            NormalizedEmail = DefaultUser.AdminEmail.ToUpper(),
            EmailConfirmed = true,
            SecurityStamp = DefaultUser.AdminSecurityStamp,
            ConcurrencyStamp = DefaultUser.AdminConcurencyStamp,
            PasswordHash = DefaultUser.AdminPasswordHash,
            PhoneNumber = "01234567890",
        });
    }
}
