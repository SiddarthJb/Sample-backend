namespace Z1.Core.Interfaces
{
    public interface IBlobService
    {
        Task<string> UploadAsync(Stream fileStream, CancellationToken cancellationToken = default);
        Task DeleteAsync(string fileId, CancellationToken cancellationToken = default);
        string GenerateSasToken(string blobName, int expiryMinutes = default);
    }
}
