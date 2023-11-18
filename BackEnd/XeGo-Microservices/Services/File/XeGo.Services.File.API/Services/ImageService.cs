using XeGo.Services.File.API.Services.IServices;

namespace XeGo.Services.File.API.Services
{
    public class ImageService : IImageService
    {
        public Image<Rgba32> Resize(Image<Rgba32> image)
        {
            image.Mutate(x => x.Resize(128, 128));
            return image;
        }
    }
}
