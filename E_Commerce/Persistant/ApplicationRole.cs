namespace E_Commerce.Persistant;

public class ApplicationRole : IdentityRole
{
    public bool isDefault { get; set; }
    public bool IsDeleted { get; set; }
}
