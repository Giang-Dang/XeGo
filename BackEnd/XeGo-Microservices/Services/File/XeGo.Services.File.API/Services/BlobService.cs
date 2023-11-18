using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using XeGo.Services.File.API.Models;
using XeGo.Services.File.API.Services.IServices;

namespace XeGo.Services.File.API.Services
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient _blobClient;

        public BlobService(BlobServiceClient blobClient)
        {
            _blobClient = blobClient;
        }
        public async Task<List<string>> GetAllBlobNames(string containerName)
        {
            BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(containerName);
            var blobs = blobContainerClient.GetBlobsAsync();

            var blobString = new List<string>();

            await foreach (var blob in blobs)
            {
                blobString.Add(blob.Name);
            }

            return blobString;
        }

        public string GetBlobAbsoluteUriWithSas(string blobName, string containerName)
        {
            BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(containerName);
            var blobClient = blobContainerClient.GetBlobClient(blobName);

            string Uri = "";

            if (blobClient.CanGenerateSasUri)
            {
                BlobSasBuilder sasBuilder = new()
                {
                    BlobContainerName = blobClient.GetParentBlobContainerClient().Name,
                    BlobName = blobClient.Name,
                    Resource = "b",
                    ExpiresOn = DateTimeOffset.UtcNow.AddDays(1)
                };

                sasBuilder.SetPermissions(BlobSasPermissions.Read);

                Uri = blobClient.GenerateSasUri(sasBuilder).AbsoluteUri;
            }

            return Uri;
        }

        public async Task<bool> UploadBlob(string blobName, IFormFile file, string containerName, Blob? blob)
        {
            BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(containerName);
            var blobClient = blobContainerClient.GetBlobClient(blobName);

            var httpHeaders = new BlobHttpHeaders()
            {
                ContentType = file.ContentType
            };

            IDictionary<string, string>? metaData = null;
            if (blob != null)
            {
                metaData = new Dictionary<string, string>();
                metaData.Add("title", blob.Title ?? "");
                metaData.Add("comment", blob.Comment ?? "");
            }

            var result = await blobClient.UploadAsync(file.OpenReadStream(), httpHeaders, metaData);

            return result != null;
        }

        public async Task<bool> DeleteBlob(string blobName, string containerName)
        {
            BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(containerName);
            var blobClient = blobContainerClient.GetBlobClient(blobName);

            return await blobClient.DeleteIfExistsAsync();
        }

        public async Task<bool> Exists(string blobName, string containerName)
        {
            return await _blobClient.GetBlobContainerClient(containerName).GetBlobClient(blobName).ExistsAsync();
        }
    }
}
