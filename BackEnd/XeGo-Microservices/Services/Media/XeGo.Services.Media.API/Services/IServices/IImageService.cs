namespace XeGo.Services.Media.API.Services.IServices
{
    public interface IImageService
    {
        Task<Image<Rgba32>> Resize(Image<Rgba32> image);
    }
}
