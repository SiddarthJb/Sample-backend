using Z1.Match.Interfaces;

namespace Z1.Match
{
    public static class Configurations
    {
        public static void ConfigureMatchServices(this IServiceCollection services)
        {
            services.AddScoped<IMatchService, MatchService>();
        }
    }
}
