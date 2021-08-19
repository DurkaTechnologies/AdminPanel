using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Extensions;

namespace WebUI.Services
{
    public static class ImageService
    {
        public static string RootPass { get; set; }

        public static string SaveImage(IFormFileCollection files)
        {
            if (RootPass != null)
            {
                IFormFile file = files.FirstOrDefault();
                //byte[] image = file.OptimizeImageSize(700, 700);

                string name = Guid.NewGuid().ToString();
                string extension = Path.GetExtension(file.FileName);
                string path = Path.Combine(RootPass + ENV.ImagePath, name + extension);

                using (var fileStream = System.IO.File.Create(path))
                {
                    //fileStream.Write(image, 0, image.Length);
                    file.CopyTo(fileStream);
                }

                return name + extension;
            }
            else
                return null;
        }

        public static void DeleteImage(string name) 
        {
            string path = Path.Combine(RootPass + ENV.ImagePath, name);

            if (File.Exists(path))
                File.Delete(path);
        }
    }
}
