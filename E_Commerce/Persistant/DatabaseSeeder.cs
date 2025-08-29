
namespace E_Commerce.Persistant;

public class DatabaseSeeder
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public DatabaseSeeder(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task SeedAsync()
    {
        // Seed Roles
        if (!await _roleManager.RoleExistsAsync(DefaultRoles.Admin))
        {
            await _roleManager.CreateAsync(new ApplicationRole
            {
                Id = DefaultRoles.AdminRoleId,
                Name = DefaultRoles.Admin,
                NormalizedName = DefaultRoles.Admin.ToUpper(),
                ConcurrencyStamp = DefaultRoles.AdminConcurencyStamp,
                isDefault = false,
                IsDeleted = false
            });
        }

        if (!await _roleManager.RoleExistsAsync(DefaultRoles.Owner))
        {
            await _roleManager.CreateAsync(new ApplicationRole
            {
                Id = DefaultRoles.OwnerRoleId,
                Name = DefaultRoles.Owner,
                NormalizedName = DefaultRoles.Owner.ToUpper(),
                ConcurrencyStamp = DefaultRoles.OwnerConcurencyStamp,
                isDefault = true,
                IsDeleted = false
            });
        }

        // Seed Admin User
        var adminUser = await _userManager.FindByEmailAsync(DefaultUser.AdminEmail);
        if (adminUser == null)
        {
            adminUser = new ApplicationUser
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
                PhoneNumber = "01234567890",
            };

            var result = await _userManager.CreateAsync(adminUser, DefaultUser.AdminPassword);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(adminUser, DefaultRoles.Admin);
            }
        }
    }
} 