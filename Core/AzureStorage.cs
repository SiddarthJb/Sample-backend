using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.Extensions.Options;
using Z1.Core.Interfaces;

namespace Z1.Core
{
    public class AzureStorage: IBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;
        private readonly string _storageAccountName;
        private readonly string _storageAccountKey;

        public AzureStorage(IOptions<AzureBlobStorageSettings> azureBlobSettings)
        {
            _blobServiceClient = new BlobServiceClient(azureBlobSettings.Value.ConnectionString);
            _containerName = azureBlobSettings.Value.ContainerName;
            _storageAccountName = azureBlobSettings.Value.StorageAccountName;
            _storageAccountKey = azureBlobSettings.Value.StorageAccountKey;
        }

        public async Task<string> UploadAsync(Stream fileStream, CancellationToken cancellationToken = default)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var fileId = Guid.NewGuid();
            BlobClient blobClient = containerClient.GetBlobClient(fileId.ToString() + ".jpg");
            await blobClient.UploadAsync(fileStream,cancellationToken);
            return fileId.ToString();
        }

        public async Task DeleteAsync(string fileId, CancellationToken cancellationToken = default)
        {
            BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            BlobClient blobClient = blobContainerClient.GetBlobClient(fileId.ToString());
            await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
        }

        public string GenerateSasToken(string blobName, int expiryMinutes = 500)
        {
            try
            {
                var blobClient = _blobServiceClient.GetBlobContainerClient(_containerName).GetBlobClient(blobName);

                var sasBuilder = new BlobSasBuilder
                {
                    BlobContainerName = _containerName,
                    BlobName = blobName,
                    Resource = "b",
                    ExpiresOn = DateTimeOffset.UtcNow.AddHours(6)
                };

                sasBuilder.SetPermissions(BlobSasPermissions.Read | BlobSasPermissions.Write);

                var sasToken = sasBuilder.ToSasQueryParameters(new StorageSharedKeyCredential(_storageAccountName, _storageAccountKey)).ToString();

                return $"{blobClient.Uri}?{sasToken}";
            }
            catch(Exception ex)
            {
                return "";
            }

        }
    }

}

