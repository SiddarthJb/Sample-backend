namespace Z1.Auth.Enums
{
    public enum AuthProvider
    {
        Google = 1,
        Facebook,
        Email
    }

    public enum UserState
    {
        PhoneVerification = 1,
        UserVerification,
        ProfileCreation,
        ExistingUser,
    }
}
