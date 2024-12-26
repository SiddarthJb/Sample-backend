using NetTopologySuite.Geometries;
using Z1.Auth.Models;
using Z1.Core.Data;

namespace Z1.Match.Models
{
    public class MatchQueue : AuditableEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public Point Location { get; set; }
        public DateTime RequestTime { get; set; }
        public Int16 Tries { get; set; }
    }
}
