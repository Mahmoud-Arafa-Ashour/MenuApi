﻿
namespace E_Commerce.Authorization;

public class PermissionAttributeHandler : AuthorizationHandler<PermissionRequiremen>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequiremen requirement)
    {
        if (context.User.Identity is not { IsAuthenticated: true } ||
             !context.User.Claims.Any(x => x.Value == requirement.Permission && x.Type == Permissions.Type))
            return ;

        context.Succeed(requirement);
        return;
    }
}
