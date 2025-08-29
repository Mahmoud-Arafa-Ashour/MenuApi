namespace E_Commerce.Authorization;

public class HasPermissionAttribute(string Permission) : AuthorizeAttribute(Permission) 
{
}
