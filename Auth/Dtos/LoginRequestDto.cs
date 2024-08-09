
using System.ComponentModel.DataAnnotations;
using Z1.Auth.Enums;

namespace Z1.Auth.Dtos
{
    public class LoginRequestDto
    {
        [EmailAddress]
        public required string Email { get; set; }
        public required AuthProvider Provider { get; set; }
        public required string Token { get; set; }
    }
}
