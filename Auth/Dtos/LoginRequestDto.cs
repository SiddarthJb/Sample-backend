
using System.ComponentModel.DataAnnotations;
using Z1.Auth.Enums;

namespace Z1.Auth.Dtos
{
    public class LoginRequestDto
    {
        public required AuthProvider Provider { get; set; }
        public required string Token { get; set; }
        public string Platform { get; set; }
    }
}
