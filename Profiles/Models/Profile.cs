using NetTopologySuite.Geometries;
using Z1.Auth.Models;
using Z1.Core.Data;

namespace Z1.Profiles.Models
{

    public class Profile : AuditableEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public required string Name { get; set; }
        public required DateOnly Dob { get; set; }
        public byte Age { get; set; }
        public Int16 Height { get; set; }
        public required int Gender { get; set; }
        public required int MaritalStatus { get; set; }
        public required int Kids { get; set; }
        public required int Education { get; set; }
        public required int Zodiac { get; set; }
        public required int Alcohol { get; set; }
        public required int Smoke { get; set; }
        public required int Religion { get; set; }
        public required int Profession { get; set; }
        public List<Languages> Languages { get; set; } = [];
        public List<Interests> Interests { get; set; } = [];
        public required Point Location { get; set; }
        public ICollection<Image> Images { get; } = new List<Image>();
        public string Bio { get; set; } = string.Empty;
        public string Work { get; set; } = string.Empty;
    }
}
