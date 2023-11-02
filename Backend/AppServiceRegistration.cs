using Backend.Services._SeederService;
using Backend.Services.AuthServices;
using Backend.Services.NotificationServices;
using Backend.Services.UserAccountService;
using Microsoft.AspNetCore.Routing.Matching;

namespace Backend
{
    public static class AppServiceRegistration
    {
        public static void AddAppServices(this IServiceCollection services)
        {
            // ADD AUTHORIZATION POLICY
            services.AddAuthorization(options =>
            {
                options.AddPolicy(AUTHORIZATION.EXCLUDE_INACTIVE, policy => policy.AddRequirements(new CustomRoleRequirement()));
            });

            // SERVICES
            services.AddScoped<SeederService>();

            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<IUserAccountService, UserAccountService>();

            services.AddScoped<INotificationService, NotificationService>();

        }

        
    }
}
