using Backend.Services._SeederService;
using Backend.Services.AuthServices;
using Backend.Services.UserAccountService;

namespace Backend
{
    public static class AppServiceRegistration
    {
        public static void AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<SeederService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserAccountService, UserAccountService>();
        }

        
    }
}
