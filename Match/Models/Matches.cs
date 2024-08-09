using Z1.Auth.Models;
using Z1.Chat.Models;
using Z1.Core.Data;

namespace Z1.Match.Models
{
    public class Matches: AuditableEntity
    {
        public int Id { get; set; }
        public User User1 { get; set; }
        public int User1Id { get; set; }
        public User User2 { get; set; }
        public int User2Id { get; set; }
        public bool? User1Liked { get; set; } = false;
        public bool? User2Liked { get; set; } = false;
        public int Skipped { get; set; } = 0;
        public DateTime? User1LikedTime { get; set; }
        public DateTime? User2LikedTime { get; set;}
        public bool IsPartial { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Message> Messages { get; } = new List<Message>();

    }
}
