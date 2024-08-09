using Z1.Auth.Dtos;
using Z1.Auth.Models;
using Z1.Core;

namespace Z1.Auth.Interfaces
{
    public interface IAuthService
    {
        //Task<BaseResponse<SignInResposeDto>> SignInWithGoogle(SignInRequestDto model);
        //Task<BaseResponse<SignInResposeDto>> SignInWithFacebook(SignInRequestDto model);
        Task<BaseResponse<LoginResposeDto>> Login(LoginRequestDto model, string ipAddress);
        Task<BaseResponse<SendOtpRequestDto>> SendOtp(SendOtpRequestDto model);
        Task<BaseResponse<LoginResposeDto>> VerifyOtp(VerifyOtpRequestDto model, string ipAddress);
        Task<BaseResponse<LoginResposeDto>> RefreshToken(string token, string ipAddress);
        void RevokeToken(string token, string ipAddress);
        IEnumerable<User> GetAll();
        User GetById(int id);
    }
}
