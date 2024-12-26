using log4net;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Z1.Auth.Dtos;
using Z1.Auth.Interfaces;
using Z1.Auth.Models;
using Z1.Auth.Utils;
using Z1.Core;
using Z1.Core.Data;
using static Google.Apis.Auth.GoogleJsonWebSignature;


namespace Z1.Auth.Services
{
    /// <summary>
    /// Class Facebook Auth Service.
    /// </summary>
    public class GoogleAuthService : IGoogleAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _context;
        private readonly GoogleSettings _googleSettings;
        private readonly ILog _logger;

        public GoogleAuthService(
            UserManager<User> userManager,
            AppDbContext context,
            IOptions<GoogleSettings> googleConfig
            )
        {
            _userManager = userManager;
            _context = context;
            _googleSettings = googleConfig.Value;
            _logger = LogManager.GetLogger(typeof(GoogleAuthService));
        }

        /// <summary>
        /// Google SignIn
        /// </summary>
        /// <param name="model">the model</param>
        /// <returns>Task&lt;BaseResponse&lt;User&gt;&gt;</returns>
        public async Task<BaseResponse<User>> GoogleSignIn(LoginRequestDto model)
        {

            Payload payload = new();

            try
            {
                payload = await ValidateAsync(model.Token, new ValidationSettings
                {
                    Audience = new[] { _googleSettings.ClientId }
                });

            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return new BaseResponse<User>()
                {
                    Errors = new List<string> { "Failed to get response" }
                };
            }

            //var userToBeCreated = new CreateUserFromSocialLoginDto
            //{
            //    FirstName = payload.GivenName,
            //    LastName = payload.FamilyName,
            //    Email = payload.Email,
            //    LoginProviderSubject = payload.Subject,
            //};

            //var user = await _userManager.CreateUserFromSocialLogin(_context, userToBeCreated, LoginProvider.Google);

            //if (user is not null)
            //    return new BaseResponse<User>(user);

            //else
            return new BaseResponse<User>()
            {
                Errors = new List<string> { "Unable to link a Local User to a Provider" }
            };
        }


    }
}
