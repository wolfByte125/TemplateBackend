using Microsoft.AspNetCore.Authorization;

namespace Backend.Utils
{
    public class CustomRoleRequirement : AuthorizationHandler<CustomRoleRequirement>, IAuthorizationRequirement
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomRoleRequirement requirement)
        {
            if(!context.User.Claims.Any())
            {
                context.Fail();
                return Task.FromResult(false);
            }

            var roles = new[] { AUTHORIZATION.UNAUTHORIZED_ACCOUNT };
            var userIsInRole = roles.Any(role => context.User.IsInRole(role));
            if (userIsInRole)
            {
                context.Fail();
                return Task.FromResult(false);
            }

            context.Succeed(requirement);
            return Task.FromResult(true);
        }
    }
}
