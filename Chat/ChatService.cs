using Microsoft.EntityFrameworkCore;
using Z1.Auth.Models;
using Z1.Chat.Interfaces;
using Z1.Chat.Models;
using Z1.Core;
using Z1.Core.Data;
using Z1.Core.Exceptions;
using Z1.Core.Interfaces;
using Z1.Profiles.Models;

namespace Z1.Chat
{
    public class ChatService : IChatService
    {
        private readonly AppDbContext _context;
        private readonly IBlobService _blobService;

        public ChatService(AppDbContext context, IBlobService blobService)
        {
            _context = context;
            _blobService = blobService;
        }

        public async Task<IEnumerable<MessageDto>> GetChatHistory(int matchId, int skip, int take)
        {
            return await _context.Messages.Select(x => new MessageDto
            {
                Id = x.Id,
                Text = x.Text,
                SenderId = x.SenderId,
                ReceiverId = x.ReceiverId,
                Timestamp = x.Timestamp,
                MatchId = x.MatchId,
                SeenBy1 = x.SeenBy1,
                SeenBy2 = x.SeenBy2,
                DeleteFor1 = x.DeleteFor1,
                DeleteFor2 = x.DeleteFor2,
            })
                .Where(x => x.MatchId == matchId)
                .OrderByDescending(m => m.Timestamp)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task<BaseResponse<List<ChatListItem>>> GetAllChats(User user)
        {
            var matches = _context.Matches
                 .Where(x => (x.User1Id == user.Id || x.User2Id == user.Id) 
                 && x.IsActive == true && ((x.IsPartial && x.CreatedAt.AddDays(1) > DateTime.UtcNow) || !x.IsPartial))
                 .Include(x => x.Messages).ToList();

            var result = new List<ChatListItem>();

            foreach (var match in matches)
            {
                var chatListItem = new ChatListItem();

                Profile profile;
                if (match.User1Id != user.Id)
                {
                    profile = _context.Profiles.Include(x => x.Images).First(x => x.UserId == match.User1Id);
                }
                else
                {
                    profile = _context.Profiles.Include(x => x.Images).First(x => x.UserId == match.User2Id);
                }

                chatListItem.ProfilePic = _blobService.GenerateSasToken(profile.Images.First().ImageUrl);
                chatListItem.Username = profile.Name;
                chatListItem.MatchedUserId = profile.UserId;
                chatListItem.MatchId = match.Id;
                chatListItem.CreatedAt = match.CreatedAt;
                chatListItem.IsPartial = match.IsPartial;
                chatListItem.Messages = await _context.Messages.Where(x => x.DeleteFor1 != user.Id && x.DeleteFor2 != user.Id).Select(x => new MessageDto
                {
                    Id = x.Id,
                    Text = x.Text,
                    SenderId = x.SenderId,
                    ReceiverId = x.ReceiverId,
                    Timestamp = x.Timestamp,
                    MatchId = x.MatchId,
                    SeenBy1 = x.SeenBy1,
                    SeenBy2 = x.SeenBy2,
                    DeleteFor1 = x.DeleteFor1,
                    DeleteFor2 = x.DeleteFor2,
                })
                .Where(x => x.MatchId == match.Id)
                .OrderByDescending(m => m.Timestamp)
                .Skip(0)
                .Take(50)
                .ToListAsync();

                result.Add(chatListItem);
            }

            var response = new BaseResponse<List<ChatListItem>>();
            response.Data = result;

            return response;
         }

        public async Task<Message> AddMessage(Message message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
            return message;
        }

        public BaseResponse<List<ChatListItem>> GetChatList(User user)
        {
            var matches = _context.Matches
                 .Where(x => (x.User1Id == user.Id || x.User2Id == user.Id) && x.IsActive == true)
                 .Include(x => x.Messages).ToList();


            List<ChatListItem> list = new List<ChatListItem>();
            foreach (var match in matches)
            {
                var unseenMessages = match.Messages.Where(x => x.SeenBy1 != user.Id && x.SeenBy2 != user.Id);
                var messageListItem = new ChatListItem();

                messageListItem.LastMessage = "";

                if (match.Messages.Count > 0)
                {
                    var lastMessage = match.Messages.Last();
                    messageListItem.LastMessage = lastMessage.Text;
                }
                var unseenCount = unseenMessages?.Count();

                messageListItem.UnreadCount = unseenCount ?? 0;

                Profile profile;
                if (match.User1Id != user.Id)
                {
                    profile = _context.Profiles.Include(x => x.Images).First(x => x.UserId == match.User1Id);
                }
                else
                {
                    profile = _context.Profiles.Include(x => x.Images).First(x => x.UserId == match.User2Id);
                }

                messageListItem.ProfilePic = _blobService.GenerateSasToken(profile.Images.First().ImageUrl);
                messageListItem.Username = profile.Name;
                messageListItem.MatchedUserId = profile.UserId;
                messageListItem.MatchId = match.Id;
                list.Add(messageListItem);
            }

            var response = new BaseResponse<List<ChatListItem>>();
            response.Data = list;

            return response;
        }

        public BaseResponse<ChatListItem> GetCurrentMatchChat(User user)
        {

            var currentMatch = _context.Matches.Include(x => x.Messages)
                .FirstOrDefault(x => (x.User1Id == user.Id || x.User2Id == user.Id)
                                && x.IsPartial && x.IsActive && x.CreatedAt.AddDays(1) > DateTime.UtcNow);

            var response = new BaseResponse<ChatListItem>();

            if (currentMatch != null)
            {
                var unseenMessages = currentMatch.Messages.Where(x => x.SeenBy1 != user.Id && x.SeenBy2 != user.Id);
                var messageListItem = new ChatListItem();

                messageListItem.LastMessage = "";

                if (currentMatch.Messages.Count > 0)
                {
                    var lastMessage = currentMatch.Messages.Last();
                    messageListItem.LastMessage = lastMessage.Text;
                }
                var unseenCount = unseenMessages?.Count();

                messageListItem.UnreadCount = unseenCount ?? 0;

                Profile profile;
                if (currentMatch.User1Id != user.Id)
                {
                    profile = _context.Profiles.Include(x => x.Images).First(x => x.UserId == currentMatch.User1Id);
                }
                else
                {
                    profile = _context.Profiles.Include(x => x.Images).First(x => x.UserId == currentMatch.User2Id);
                }

                messageListItem.ProfilePic = _blobService.GenerateSasToken(profile.Images.First().ImageUrl);
                messageListItem.Username = profile.Name;
                messageListItem.MatchedUserId = profile.UserId;
                messageListItem.MatchId = currentMatch.Id;
                messageListItem.CreatedAt = currentMatch.CreatedAt;
                response.Data = messageListItem;
            }

            return response;
        }

        public BaseResponse<bool> MarkAsRead (User user , long messageId)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var lstMessage = _context.Messages.FirstOrDefault(x => x.Id == messageId);

                if (lstMessage != null)
                {
                    var messages = _context.Messages.Where(x => x.MatchId == lstMessage.MatchId && x.Timestamp <= lstMessage.Timestamp && (x.SeenBy1 != user.Id && x.SeenBy2 != user.Id)).ToList();
                    foreach (var message in messages)
                    {
                        if (message.SeenBy1 == null) { 
                            message.SeenBy1 = user.Id;
                        }
                        else
                        {
                            message.SeenBy2 = user.Id;
                        }
                    }

                    _context.SaveChanges();
                    response.Data = true;
                }
            }
            catch (Exception ex)
            {
                response.Data = false;
                throw new AppException("Something went wrong.");
            }



            return response;
        }

        public async Task<BaseResponse<bool>> DeleteHistory(User user, int matchId, int lastMessageId)
        {
            BaseResponse<bool> response = new();

            var chatHistory = _context.Messages.Where(x => x.MatchId == matchId && x.IsActive && x.Id <= lastMessageId);

            if (chatHistory.Count() > 0)
            {
                foreach (var chat in chatHistory)
                {
                    if(chat.DeleteFor1 == null)
                    {
                        chat.DeleteFor1 = user.Id;
                    }
                    else
                    {
                        chat.DeleteFor2 = user.Id;
                    }
                }

                await _context.SaveChangesAsync();
                response.Data = true;
            }

            return response;
        }
    }
}
