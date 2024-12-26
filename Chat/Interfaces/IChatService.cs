using Z1.Auth.Models;
using Z1.Chat.Models;
using Z1.Core;

namespace Z1.Chat.Interfaces
{
    public interface IChatService
    {
        Task<IEnumerable<MessageDto>> GetChatHistory(int matchId, int skip, int take);
        Task<Message> AddMessage(Message message);
        BaseResponse<List<ChatListItem>> GetChatList(User user);
        Task<BaseResponse<List<ChatListItem>>> GetAllChats(User user);
        Task<BaseResponse<bool>> MarkAsReadAsync(User user, long messageId);
        BaseResponse<ChatListItem> GetCurrentMatchChat(User user);
        Task<BaseResponse<bool>> DeleteHistory(User user, int matchId, int lastMessageId);
    }
}
