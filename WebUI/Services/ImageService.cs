using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Net;

namespace WebUI.Services
{
	public static class ImageService
	{
		public static string RootPass { get; set; }

		public static string SaveImageLocal(IFormFile formFile, string extension = null)
		{
			if (RootPass != null)
			{
				try
				{
					string name = Guid.NewGuid().ToString();
					if (String.IsNullOrEmpty(extension))
						extension = Path.GetExtension(formFile.FileName);

					string path = Path.Combine(RootPass + ENV.UploadPath, name + extension);

					using (var fileStream = File.Create(path))
					{
						formFile.CopyTo(fileStream);
					}
					return name + extension;
				}
				catch (Exception)
				{
					return null;
				}
			}
			return null;
		}

		public static bool DeleteImageLocal(string name)
		{
			if (name == "default-user.png")
				return false;

			string path = Path.Combine(RootPass + ENV.UploadPath, name);

			try
			{
				if (File.Exists(path))
					File.Delete(path);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}
