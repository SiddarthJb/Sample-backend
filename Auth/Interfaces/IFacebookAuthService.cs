using IdentityModel.Jwk;
using Z1.Auth.Dtos;
using Z1.Core;

namespace Z1.Auth.Interfaces
{
    public interface IFacebookAuthService
    {
        Task<FacebookTokenValidationResponse?> ValidateFacebookToken(string accessToken);
        Task<BaseResponse<FacebookUserInfoResponse>> GetFacebookUserInformation(string accessToken);
        Task<JsonWebKeySet> GetFacebookKeysAsync();
    } 
}
