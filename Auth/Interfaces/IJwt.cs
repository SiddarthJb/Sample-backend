using Z1.Auth.Models;

namespace Z1.Auth.Interfaces
{
    public interface IJwt
    {
        public string GenerateJwtToken(User user);
        public int? ValidateJwtToken(string token);
        public RefreshToken GenerateRefreshToken(string ipAddress);
    }
}
