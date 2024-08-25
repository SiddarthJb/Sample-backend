namespace Z1.Core.Configurations
{
    public static class AppSettingsConfig
    {
        public static void ConfigureAppSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<ConnectionStringsSettings>()
                .Bind(configuration.GetSection(ConnectionStringsSettings.Key));

            services.AddOptions<JwtSettings>().Bind(configuration.GetSection(JwtSettings.Key));

            services.AddOptions<GoogleSettings>().Bind(configuration.GetSection(GoogleSettings.Key));
            services.AddOptions<AzureBlobStorageSettings>().Bind(configuration.GetSection(AzureBlobStorageSettings.Key));
            services.AddOptions<GeneralSettings>().Bind(configuration.GetSection(GeneralSettings.Key));

        }
    }
}
