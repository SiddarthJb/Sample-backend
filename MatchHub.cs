using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Z1.Chat.Interfaces;
using Z1.Chat.Models;
using Z1.Match.Interfaces;

namespace Z1
{
    [Authorize]
    public class MatchHub : Hub
    {
        private readonly IMatchService _matchService;
        private readonly IChatService _chatService;


        public MatchHub(IMatchService matchService, IChatService chatService)
        {
            _matchService = matchService;
            _chatService = chatService;
        }

        public async Task StartMatching(string userId, string latitude, string longitude)
        {
            var status = await _matchService.AddToMatchQueue(userId, latitude, longitude);

            var matcher = new MatchResponse()
            {
                Status = status,
            };

            await Clients.Caller.SendAsync("Matcher", matcher);
            await Clients.All.SendAsync("MatchQueueCount", _matchService.GetMatchQueueCount());
        }

        public async Task SendMessage(NewMessage message)
        {
            var newMessage = new Message
            {
                MatchId = message.MatchId,
                SenderId = message.SenderId,
                ReceiverId = message.ReceiverId,
                Text = message.Text.Trim(),
                Timestamp = DateTime.UtcNow,
                IsImage = message.IsImage,
                ImageUrl = message.ImageUrl ?? "",
                IsVoiceMessage = message.IsVoiceMessage,
                VoiceMessageUrl = message.VoiceMessageUrl ?? ""
            };

            var addedMessage = await _chatService.AddMessage(newMessage);

            await Clients.User(message.ReceiverId.ToString()).SendAsync("ReceiveMessage", addedMessage);
            await Clients.User(message.SenderId.ToString()).SendAsync("ReceiveMessage", addedMessage);
        }

        public async Task Typing(int userId)
        {
            await Clients.All.SendAsync("Typing", userId);
        }

        // Models

        public class MatchResponse {
            public string Status { get; set; }
        }
        
    }
}
