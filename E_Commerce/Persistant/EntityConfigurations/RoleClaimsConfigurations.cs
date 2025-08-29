﻿namespace E_Commerce.Persistant.EntityConfigurations;
public class RoleClaimsConfigurations : IEntityTypeConfiguration<IdentityRoleClaim<string>>
{
    public void Configure(EntityTypeBuilder<IdentityRoleClaim<string>> builder)
    {
        var Permission = Permissions.GetAllPermissions;
        var adminClaims = new List<IdentityRoleClaim<string>>();
        for(int i = 0; i < Permission.Count; i++)
        {
            adminClaims.Add(new IdentityRoleClaim<string>()
            {
                Id = i + 1,
                ClaimType = Permissions.Type,
                ClaimValue = Permission[i],
                RoleId = DefaultRoles.AdminRoleId,
            });
        }
        builder.HasData(adminClaims);
    }
}
