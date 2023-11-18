namespace XeGo.Services.File.API.Services.IServices
{
    public interface IImageService
    {
        Image<Rgba32> Resize(Image<Rgba32> image);
    }
}
