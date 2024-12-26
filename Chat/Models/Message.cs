using Z1.Core.Data;
using Z1.Match.Models;

namespace Z1.Chat.Models
{
    public class Message : AuditableEntity
    {
        public long Id { get; set; }
        public required int SenderId { get; set; }
        public required int ReceiverId { get; set; }
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsVoiceMessage { get; set; }
        public string VoiceMessageUrl { get; set; }
        public bool IsImage { get; set; }
        public string ImageUrl { get; set; }
        public int ReplyToId { get;set; }
        public int MatchId { get; set; }
        public Matches Match { get; set; } = null!;
        public int? SeenBy1 { get; set; }
        public int? SeenBy2 { get; set; }
        public int? DeleteFor1 { get; set; }
        public int? DeleteFor2 { get; set; }    
    }
}
