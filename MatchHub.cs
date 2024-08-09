using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Z1.Auth.Models;
using Z1.Chat;
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
            await _matchService.AddToMatchQueue(userId, latitude, longitude);
            await Clients.Caller.SendAsync("SearchingForMatch", "Searching for a match...");
        }

        public async Task SendMessage(NewMessage message)
        {
            var newMessage = new Message
            {
                MatchId = message.MatchId,
                SenderId = message.SenderId,
                ReceiverId = message.ReceiverId,
                Text = message.Text.Trim(),
                Timestamp = DateTime.Now,
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
    }
}
