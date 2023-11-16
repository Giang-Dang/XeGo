using XeGo.Services.Media.API.Models;

namespace XeGo.Services.Media.API.Services.IServices
{
    public interface IBlobService
    {
        Task<List<string>> GetAllBlobNames(string containerName);
        string GetBlobAbsoluteUriWithSas(string blobName, string containerName);
        Task<bool> UploadBlob(string blobName, IFormFile file, string containerName, Blob? blob);
        Task<bool> DeleteBlob(string blobName, string containerName);
        Task<bool> Exists(string blobName, string containerName);
    }
}
