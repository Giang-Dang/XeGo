using XeGo.Services.Media.API.Services.IServices;

namespace XeGo.Services.Media.API.Services
{
    public class ImageService : IImageService
    {
        public async Task<Image<Rgba32>> Resize(Image<Rgba32> image)
        {
            image.Mutate(x => x.Resize(128, 128));
            return image;
        }
    }
}
