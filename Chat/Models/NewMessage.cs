using Z1.Core.Data;

namespace Z1.Chat.Models
{
    public class NewMessage
    {
        public int MatchId { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string Text { get; set; }
        public bool IsVoiceMessage { get; set; }
        public string? VoiceMessageUrl { get; set; }
        public bool IsImage { get; set; }
        public string? ImageUrl { get; set; }
    }

    public class MessageDto {
        public long Id { get; set; }
        public int MatchId { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsSeen { get; set; }
    }

}
