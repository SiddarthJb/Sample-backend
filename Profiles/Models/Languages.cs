using Z1.Core.Data;

namespace Z1.Profiles.Models
{
    public class Languages : AuditableEntity
    {
        public int Id { get; set; }
        public required string Language { get; set; }
        public List<Profile> Profiles { get; } = [];
    }
}
