using Z1.Auth.Enums;
using Z1.Core.Data;

namespace Z1.Auth.Models
{
    public class PendingUser : AuditableEntity
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required AuthProvider AuthProvider { get; set; }
        public string PhoneNo { get; set; } = string.Empty;
        public string CountryCode { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }
}
