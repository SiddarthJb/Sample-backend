using Microsoft.AspNetCore.SignalR;

namespace Z1.Match
{
    public class UserIdProvider : IUserIdProvider
    {
        public virtual string GetUserId(HubConnectionContext connection)
        {
            return connection.User?.FindFirst("id")?.Value!;
        }
    }
}
