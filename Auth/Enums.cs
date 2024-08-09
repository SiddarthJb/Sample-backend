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
        ExistingUser = 1,
        PhoneVerification,
        ProfileCreation
    }
}
