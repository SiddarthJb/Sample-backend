using Z1.Profiles.Interfaces;

namespace Z1.Profiles
{
    public static class Configurations
    {
        public static void ConfigureProfileServices(this IServiceCollection services)
        {
            services.AddScoped<IProfileService, ProfileService>();
        }
    }
}
