namespace Z1.Core
{

    public class ConnectionStringsSettings
    {
        public const string Key = "ConnectionStrings";
        public string DefaultConnection { get; set; } = null!;
    }

    public class JwtSettings
    {
        public const string Key = "Jwt";
        public string ValidIssuer { get; set; } = null!;
        public string ValidAudience { get; set; } = null!;
        public string Secret { get; set; } = null!;
        public string DurationInMinutes { get; set; } = null!;
        public string RefreshTokenTTL { get; set; } = null!;
    }

    public class GoogleSettings
    {
        public const string Key = "Google";
        public string ClientId { get; set; } = null!;
        public string ClientSecret { get; set; } = null!;
    }

    public class FacebookAuthSettings
    {
        public const string Key = "FaceBook";
        public string TokenValidationUrl { get; set; }
        public string UserInfoUrl { get; set; }
        public string AppId { get; set; }
        public string AppSecret { get; set; }
    }

    public class AzureBlobStorageSettings
    {
        public const string Key = "AzureBlobStorage";
        public string ConnectionString { get; set; } = null!;
        public string ContainerName { get; set; } = null!;
        public string StorageAccountName { get; set; } = null!;
        public string StorageAccountKey { get; set; } = null!;
    }

    public class GeneralSettings
    {
        public const string Key = "General";
        public double MatchQueueTimeOut { get; set; }
        public int FreePerDayMatchLimit { get; set; }
    }

}
