using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;
using Z1.Auth.Enums;
using Z1.Match.Models;
using Z1.Profiles.Models;

namespace Z1.Auth.Models
{
    public class User : IdentityUser<int>
    {
        public string? CountryCode { get; set; }
        public Profile? Profile { get; set; }
        public required AuthProvider AuthProvider { get; set; }

        [JsonIgnore]
        public List<RefreshToken> RefreshTokens { get; set; }
        //public ICollection<Matches> Matches { get; } = new List<Matches>();
        public ICollection<MatchQueue> MatchQueue { get; } = new List<MatchQueue>();
        public bool IsSubscribed { get; set; } = false;
        public int Keys { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class Role : IdentityRole<int>
    {
    }

    public class UserClaim : IdentityUserClaim<int> { }

    public class UserRole : IdentityUserRole<int> { }

    public class UserLogin : IdentityUserLogin<int> { }

    public class RoleClaim : IdentityRoleClaim<int> { }

    public class UserToken : IdentityUserToken<int> { }
}
