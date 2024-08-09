using System.ComponentModel.DataAnnotations;

namespace Z1.Auth.Dtos
{
    public class SendOtpRequestDto
    {
        [EmailAddress]
        public required string Email { get; set; }
        [Phone]
        public required string PhoneNumber { get; set; }
        public required string CountryCode { get; set; }
    }
}
