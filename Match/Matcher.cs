
using Z1.Match.Interfaces;

namespace Z1.Match
{
    public class Matcher : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public Matcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var matchingService = scope.ServiceProvider.GetRequiredService<IMatchService>();
                    await matchingService.Search();
                }

                // Wait for a period before checking the queue again
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
    }
}
