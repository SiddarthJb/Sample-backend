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
        public bool Subscribed { get; set; } = false;
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
