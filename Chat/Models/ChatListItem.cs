namespace Z1.Chat.Models
{
    public class ChatListItem
    {
        public int MatchId { get; set; }
        public int MatchedUserId { get; set; }
        public string ProfilePic { get; set; }
        public string Username { get; set;}
        public string LastMessage { get; set; }
        public int UnreadCount { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool IsPartial { get; set; }
        public List<MessageDto> Messages { get; set; }
    }
}
