using System.ComponentModel.DataAnnotations;

namespace Z1.Auth.Dtos
{
    public class RefreshRequestDto
    {
        [Required]
        public string Refresh { get; set; } = null!;
    }
}
