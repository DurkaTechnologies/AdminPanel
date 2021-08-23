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

				string name = Guid.NewGuid().ToString();
				string extension = Path.GetExtension(file.FileName);
				string path = Path.Combine(RootPass + ENV.ImagePath, name + extension);

				using (var fileStream = System.IO.File.Create(path))
				{
					file.CopyTo(fileStream);
				}

				return name + extension;
			}
			return null;
		}
		public static string SaveImage(IFormFile file, string extension = null)
		{
			if (RootPass != null)
			{
				string name = Guid.NewGuid().ToString();
				if (String.IsNullOrEmpty(extension))
					extension = Path.GetExtension(file.FileName);

				string path = Path.Combine(RootPass + ENV.ImagePath, name + extension);

				using (var fileStream = System.IO.File.Create(path))
				{
					file.CopyTo(fileStream);
				}

				return name + extension;
			}
			return null;
		}

		public static void DeleteImage(string name)
		{
			if (name == "default-user.png")
				return;

			string path = Path.Combine(RootPass + ENV.ImagePath, name);

			if (File.Exists(path))
				File.Delete(path);
		}
	}
}
