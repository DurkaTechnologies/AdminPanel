using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.IO;

namespace WebUI.Extensions
{
    public static class FormFileExtensions
    {
        public static byte[] OptimizeImageSize(this IFormFile file, int maxWidth, int maxHeight)
        {
            using (var stream = file.OpenReadStream())
            using (var image = Image.Load(stream))
            {
                using (var writeStream = new MemoryStream())
                {
                    if (image.Height > maxHeight)
                    {
                        var thumbScaleFactor = maxHeight / image.Height;
                        image.Mutate(i => i.Resize(maxHeight, image.Width *
                            thumbScaleFactor));
                    }
                    image.SaveAsPng(writeStream);
                    return writeStream.ToArray();
                }
            }
        }
    }
}
