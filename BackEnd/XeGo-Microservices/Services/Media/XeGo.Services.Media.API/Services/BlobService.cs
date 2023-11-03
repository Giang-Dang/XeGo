using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using XeGo.Services.Media.API.Models;
using XeGo.Services.Media.API.Services.IServices;

namespace XeGo.Services.Media.API.Services
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

        public string GetBlobAbsoluteUri(string blobName, string containerName)
        {
            BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(containerName);
            var blobClient = blobContainerClient.GetBlobClient(blobName);

            return blobClient.Uri.AbsoluteUri;
        }

        public async Task<bool> UploadBlob(string blobName, IFormFile file, string containerName, Blob blob)
        {
            BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(containerName);
            var blobClient = blobContainerClient.GetBlobClient(blobName);

            var httpHeaders = new BlobHttpHeaders()
            {
                ContentType = file.ContentType
            };

            IDictionary<string, string> metaData = new Dictionary<string, string>();
            metaData.Add("title", blob.Title ?? "");
            metaData.Add("comment", blob.Comment ?? "");


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
