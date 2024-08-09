using Z1.Chat.Interfaces;

namespace Z1.Chat
{
    public static class Configurations
    {
        public static void ConfigureChatServices(this IServiceCollection services)
        {
            services.AddScoped<IChatService, ChatService>();
        }
    }
}
