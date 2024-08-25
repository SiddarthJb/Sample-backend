using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Net;
using Z1.Auth.Dtos;
using Z1.Auth.Enums;
using Z1.Auth.Interfaces;
using Z1.Auth.Models;
using Z1.Core;
using Z1.Core.Data;
using Z1.Core.Exceptions;
using Z1.Profiles.Models;

namespace Z1.Auth.Services
{
    /// <summary>
    /// Class Auth Service.
    /// </summary>
    public class AuthServices : IAuthService
    {
        private readonly AppDbContext _context;
        //private readonly IGoogleAuthService _googleAuthService;
        //private readonly IFacebookAuthService _facebookAuthService;
        private readonly UserManager<User> _userManager;
        private readonly IJwt _jwt;
        private readonly JwtSettings _jwtSettings;

        public AuthServices(
            AppDbContext context,
            //IGoogleAuthService googleAuthService,
            //IFacebookAuthService facebookAuthService,
            UserManager<User> userManager,
            IJwt jwt,
            IOptions<JwtSettings> jwtSettings)
        {
            _context = context;
            //_googleAuthService = googleAuthService;
            //_facebookAuthService = facebookAuthService;
            _userManager = userManager;
            _jwt = jwt;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<BaseResponse<LoginResposeDto>> Login(LoginRequestDto model, string ipAddress)
        {
            try
            {
                // 1st validate token - To Do
                if (model.Provider != AuthProvider.Email)
                {
                    throw new AppException("Provider not correct");
                }

                var user = await _context.Users.SingleOrDefaultAsync(x => x.Email == model.Email).ConfigureAwait(false);

                var response = new BaseResponse<LoginResposeDto>();

                if (user == null)
                {
                    var pendingUser = await _context.PendingUsers.SingleOrDefaultAsync(x => x.Email == model.Email).ConfigureAwait(false);

                    if (pendingUser != null)
                    {
                        response.Data = new LoginResposeDto() { UserState = UserState.PhoneVerification, Email = model.Email };
                        return response;
                    }

                    var newPendingUser = new PendingUser()
                    {
                        Email = model.Email,
                        Name = "",
                        AuthProvider = AuthProvider.Email,
                    };

                    await _context.PendingUsers.AddAsync(newPendingUser);
                    await _context.SaveChangesAsync();

                    response.Data = new LoginResposeDto() { UserState = UserState.PhoneVerification, Email = model.Email };
                    return response;
                }

                // authentication successful so generate jwt and refresh tokens
                var access = _jwt.GenerateJwtToken(user);
                var refresh = _jwt.GenerateRefreshToken(ipAddress);
                user.RefreshTokens.Add(refresh);

                // remove old refresh tokens from user
                removeOldRefreshTokens(user);

                // save changes to db
                _context.Update(user);
                await _context.SaveChangesAsync();

                var userState = UserState.ExistingUser;

                var profile = await _context.Profiles.FirstOrDefaultAsync(x => x.UserId == user.Id);
                if (profile == null)
                {
                    userState = UserState.ProfileCreation;
                }

                response.Data = new LoginResposeDto()
                {
                    UserState = userState,
                    UserId = user.Id.ToString(),
                    Email = model.Email,
                    AccessToken = access,
                    RefreshToken = refresh.Token
                };

                response.Status = (int)HttpStatusCode.OK;
                return response;
            }
            catch (Exception ex)
            {
                throw new AppException("Something went wrong, Please try again later");
            }
        }

        public async Task<BaseResponse<SendOtpRequestDto>> SendOtp(SendOtpRequestDto model)
        {
            var pendingUser = await _context.PendingUsers.SingleOrDefaultAsync(x => x.Email == model.Email);

            if (pendingUser == null)
            {
                throw new NotFoundException("User not found");
            }

            pendingUser.PhoneNo = model.PhoneNumber;
            pendingUser.CountryCode = model.CountryCode;

            var otp = GenerateRandomOTP(4);

            pendingUser.Code = otp;

            _context.Update(pendingUser);
            await _context.SaveChangesAsync();

            //To Do  - Send otp

            var response = new BaseResponse<SendOtpRequestDto>();
            response.Data = model;
            return response;
        }

        public async Task<BaseResponse<LoginResposeDto>> VerifyOtp(VerifyOtpRequestDto model, string ipAddress)
        {
            var pendingUser = await _context.PendingUsers.SingleOrDefaultAsync(x => x.Email == model.email);

            if(pendingUser == null)
            {
                throw new NotFoundException("User not found");
            }

            var response = new BaseResponse<LoginResposeDto>();

            if (pendingUser.Code == model.otp)
            {
                var user = new User()
                {
                    Email = pendingUser.Email,
                    EmailConfirmed = true,
                    PhoneNumber = pendingUser.PhoneNo,
                    PhoneNumberConfirmed = true,
                    CountryCode = pendingUser.CountryCode,
                    AuthProvider = pendingUser.AuthProvider,
                    RefreshTokens = []
                };

                await _userManager.CreateAsync(user);
                //await _context.Users.AddAsync(user);

                _context.PendingUsers.Remove(pendingUser);

                // otp verification successful so generate jwt and refresh tokens
                var access = _jwt.GenerateJwtToken(user);
                var refresh = _jwt.GenerateRefreshToken(ipAddress);
                user.RefreshTokens.Add(refresh);

                // remove old refresh tokens from user
                removeOldRefreshTokens(user);

                // save changes to db
                _context.Update(user);
                await _context.SaveChangesAsync();

                response.Data = new LoginResposeDto()
                {
                    UserState = UserState.ExistingUser,
                    Email = pendingUser.Email,
                    UserId = user.Id.ToString(),
                    AccessToken = access,
                    RefreshToken = refresh.Token
                };
            }
            else
            {
                response.Message = "Not Match";
            }
            return response;
        }

        public async Task<BaseResponse<LoginResposeDto>> RefreshToken(string token, string ipAddress)
        {
            var user = getUserByRefreshToken(token);
            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            if (refreshToken.IsRevoked)
            {
                // revoke all descendant tokens in case this token has been compromised
                revokeDescendantRefreshTokens(refreshToken, user, ipAddress, $"Attempted reuse of revoked ancestor token: {token}");
                _context.Update(user);
                await _context.SaveChangesAsync();
            }

            if (!refreshToken.IsActive)
                throw new Exception("Invalid token");

            // replace old refresh token with a new one (rotate token)
            var newRefreshToken = rotateRefreshToken(refreshToken, ipAddress);
            user.RefreshTokens.Add(newRefreshToken);

            // remove old refresh tokens from user
            removeOldRefreshTokens(user);

            // save changes to db
            _context.Update(user);
            await _context.SaveChangesAsync();

            // generate new jwt
            var access = _jwt.GenerateJwtToken(user);

            var response = new BaseResponse<LoginResposeDto>();
            response.Data = new LoginResposeDto()
            {
                UserState = UserState.ExistingUser,
                Email = user.Email,
                UserId = user.Id.ToString(),
                AccessToken = access,
                RefreshToken = newRefreshToken.Token
            };
            return response;
        }

        public void RevokeToken(string token, string ipAddress)
        {
            var user = getUserByRefreshToken(token);
            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            if (!refreshToken.IsActive)
                throw new Exception("Invalid token");

            // revoke token and save
            revokeRefreshToken(refreshToken, ipAddress, "Revoked without replacement");
            _context.Update(user);
            _context.SaveChanges();
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users;
        }

        public User GetById(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null) throw new KeyNotFoundException("User not found");
            return user;
        }

        // helper methods

        private User getUserByRefreshToken(string token)
        {
            var user = _context.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user == null)
                throw new Exception("Invalid token");

            return user;
        }

        private RefreshToken rotateRefreshToken(RefreshToken refreshToken, string ipAddress)
        {
            var newRefreshToken = _jwt.GenerateRefreshToken(ipAddress);
            revokeRefreshToken(refreshToken, ipAddress, "Replaced by new token", newRefreshToken.Token);
            return newRefreshToken;
        }

        private void removeOldRefreshTokens(User user)
        {
            // remove old inactive refresh tokens from user based on TTL in app settings
            user.RefreshTokens.RemoveAll(x =>
                !x.IsActive &&
                x.Created.AddDays(Convert.ToDouble(_jwtSettings.RefreshTokenTTL)) <= DateTime.UtcNow);
        }

        private void revokeDescendantRefreshTokens(RefreshToken refreshToken, User user, string ipAddress, string reason)
        {
            // recursively traverse the refresh token chain and ensure all descendants are revoked
            if (!string.IsNullOrEmpty(refreshToken.ReplacedByToken))
            {
                var childToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken.ReplacedByToken);

                if (childToken.IsActive)
                    revokeRefreshToken(childToken, ipAddress, reason);
                else
                    revokeDescendantRefreshTokens(childToken, user, ipAddress, reason);
            }
        }

        private void revokeRefreshToken(RefreshToken token, string ipAddress, string? reason = null, string? replacedByToken = null)
        {
            token.Revoked = DateTime.UtcNow;
            token.RevokedByIp = ipAddress;
            token.ReasonRevoked = reason;
            token.ReplacedByToken = replacedByToken;
        }

        private string GenerateRandomOTP(int iOTPLength)
        {

            string sOTP = String.Empty;

            string sTempChars = String.Empty;

            string[] saAllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };

            Random rand = new Random();

            for (int i = 0; i < iOTPLength; i++)

            {

                int p = rand.Next(0, saAllowedCharacters.Length);

                sTempChars = saAllowedCharacters[rand.Next(0, saAllowedCharacters.Length)];

                sOTP += sTempChars;

            }

            return sOTP;

        }
#if false

        /// <summary>
        /// Google SignIn 
        /// </summary>
        /// <param name="model">the view model</param>
        /// <returns>Task&lt;BaseResponse&lt;JwtResponseVM&gt;&gt;</returns>
        public async Task<BaseResponse<SignInResposeDto>> SignInWithGoogle(SignInRequestDto model)
        {

            var response = await _googleAuthService.GoogleSignIn(model);

            if (response.Errors.Any())
                return new BaseResponse<SignInResposeDto>(response.ResponseMessage, response.Errors);

            var jwtResponse = CreateJwtToken(response.Data);

            var data = new SignInResposeDto
            {
                Token = jwtResponse,
            };

            return new BaseResponse<SignInResposeDto>(data);
        }

        /// <summary>
        /// Facebook SignIn
        /// </summary>
        /// <param name="model">the view model</param>
        /// <returns>Task&lt;BaseResponse&lt;JwtResponseVM&gt;&gt;</returns>
        public async Task<BaseResponse<SignInResposeDto>> SignInWithFacebook(SignInRequestDto model)
        {
            var validatedFbToken = await _facebookAuthService.ValidateFacebookToken(model.AccessToken);

            if (validatedFbToken.Errors.Any())
                return new BaseResponse<JwtResponseVM>(validatedFbToken.ResponseMessage, validatedFbToken.Errors);

            var userInfo = await _facebookAuthService.GetFacebookUserInformation(model.AccessToken);

            if (userInfo.Errors.Any())
                return new BaseResponse<JwtResponseVM>(null, userInfo.Errors);

            var userToBeCreated = new CreateUserFromSocialLogin
            {
                FirstName = userInfo.Data.FirstName,
                LastName = userInfo.Data.LastName,
                Email = userInfo.Data.Email,
                ProfilePicture = userInfo.Data.Picture.Data.Url.AbsoluteUri,
                LoginProviderSubject = userInfo.Data.Id,
            };

            var user = await _userManager.CreateUserFromSocialLogin(_context, userToBeCreated, LoginProvider.Facebook);

            if (user is not null)
            {
                var jwtResponse = CreateJwtToken(user);

                var data = new JwtResponseVM
                {
                    Token = jwtResponse,
                };

                return new BaseResponse<JwtResponseVM>(data);
            }

            return new BaseResponse<JwtResponseVM>(null, userInfo.Errors);

        }
#endif

    }
}
