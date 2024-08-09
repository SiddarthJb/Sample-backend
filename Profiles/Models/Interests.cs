using Z1.Core.Data;

namespace Z1.Profiles.Models
{
    public class Interests : AuditableEntity
    {
        public int Id { get; set; }
        public required string Interest { get; set; }
        public List<Profile> Profiles { get; } = [];
    }
}
