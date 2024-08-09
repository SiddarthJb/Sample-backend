using System.ComponentModel;

namespace Z1.Core
{
    public static class Extensions
    {
        static public string GetDescription(this Enum enumValue)
        {
            var field = enumValue.GetType().GetField(enumValue.ToString());
            if (field == null)
                return enumValue.ToString();

            var attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
            {
                return attribute.Description;
            }

            return enumValue.ToString();
        }

        /// <summary>
        /// Creates user from social login
        /// </summary>
        /// <param name="userManager">the usermanager</param>
        /// <param name="context">the context</param>
        /// <param name="model">the model</param>
        /// <param name="loginProvider">the login provider</param>
        /// <returns>System.Threading.Tasks.Task&lt;User&gt;</returns>

        //public static async Task<User> CreateUserFromSocialLogin(this UserManager<User> userManager, AppDbContext context, CreateUserFromSocialLoginDto model, LoginProvider loginProvider)
        //{
        //    //CHECKS IF THE USER HAS NOT ALREADY BEEN LINKED TO AN IDENTITY PROVIDER
        //    var user = await userManager.FindByLoginAsync(loginProvider.GetDisplayName(), model.LoginProviderSubject);

        //    if (user is not null)
        //        return user; //USER ALREADY EXISTS.

        //    user = await userManager.FindByEmailAsync(model.Email);

        //    if (user is null)
        //    {
        //        user = new User
        //        {
        //            Email = model.Email,
        //        };

        //        await userManager.CreateAsync(user);

        //        //EMAIL IS CONFIRMED; IT IS COMING FROM AN IDENTITY PROVIDER
        //        user.EmailConfirmed = true;

        //        await userManager.UpdateAsync(user);
        //        await context.SaveChangesAsync();
        //    }

        //    UserLoginInfo? userLoginInfo = null;
        //    switch (loginProvider)
        //    {
        //        case LoginProvider.Google:
        //            {
        //                userLoginInfo = new UserLoginInfo(loginProvider.GetDisplayName(), model.LoginProviderSubject, loginProvider.GetDisplayName().ToUpper());
        //            }
        //            break;
        //        case LoginProvider.Facebook:
        //            {
        //                userLoginInfo = new UserLoginInfo(loginProvider.GetDisplayName(), model.LoginProviderSubject, loginProvider.GetDisplayName().ToUpper());
        //            }
        //            break;
        //        default:
        //            break;
        //    }

        //    //ADDS THE USER TO AN IDENTITY PROVIDER
        //    var result = await userManager.AddLoginAsync(user, userLoginInfo);

        //    if (result.Succeeded)
        //        return user;

        //    else
        //        return null;
        //}
    }
}
