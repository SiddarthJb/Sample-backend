using Z1.Auth.Enums;
using Z1.Auth.Models;

namespace Z1.Auth.Dtos
{
    public class LoginResposeDto
    {
        public UserState UserState { get; set; }
        public string? UserId { get; set; }
        public string Email { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
