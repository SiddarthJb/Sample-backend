using Z1.Auth.Interfaces;
using Z1.Auth.Services;
using Z1.Auth.Utils;

namespace Z1.Auth
{
    public static class Configurations
    {
        public static void ConfigureAuthServices(this IServiceCollection services)
        {
            services.AddScoped<IFacebookAuthService, FacebookAuthService>();
            services.AddScoped<IAuthService, AuthServices>();
            services.AddScoped<IJwt, Jwt>();
        }
    }
}
