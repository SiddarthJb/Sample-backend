using System.ComponentModel.DataAnnotations;

namespace Z1.Auth.Dtos
{
    public class VerifyOtpRequestDto
    {
        [EmailAddress]
        public required string email { get; set; }

        public required string otp { get; set; }
    }
}
