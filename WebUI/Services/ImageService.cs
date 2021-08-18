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
		public static string SaveImage(IFormFileCollection files, string upload)
		{

			IFormFile file = files.FirstOrDefault();
			//byte[] image = file.OptimizeImageSize(700, 700);

			string name = Guid.NewGuid().ToString();
			//string extension = Path.GetExtension(file.FileName);
			string path = Path.Combine(upload + ENV.ImagePath, file.FileName);

			using (var fileStream = System.IO.File.Create(path))
			{
				//fileStream.Write(image, 0, image.Length);
				file.CopyTo(fileStream);
			}

			return file.FileName;
		}
	}
}
