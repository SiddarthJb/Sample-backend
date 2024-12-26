using System.ComponentModel;
using Z1.Auth.Models;

namespace Z1.Core.Data
{
    public class AuditableEntity
    {
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        [DefaultValue(true)]
        public bool IsActive { get; set; } = true;
    }

}
