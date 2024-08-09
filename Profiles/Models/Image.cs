using Z1.Core.Data;

namespace Z1.Profiles.Models
{
    public class Image: AuditableEntity
    {
        public int Id { get; set; }
        public required int ProfileId { get; set; }
        public required Profile Profile { get; set; }
        public required string ImageUrl { get; set; }
        public int Order { get; set; }
        public bool IsBlurred { get; set; } = false;
        public bool IsSmall { get; set; } = false;
        public bool IsActive { get; set; } = true;
    }
}
