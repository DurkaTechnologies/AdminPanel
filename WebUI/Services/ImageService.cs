﻿using Microsoft.AspNetCore.Http;
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
				string extension = Path.GetExtension(file.FileName);
				string imgPath = Path.Combine(upload + ENV.ImagePath, name + extension);

				using (var fileStream = System.IO.File.Create(imgPath))
				{
					file.CopyTo(fileStream);
				}

				return name + extension;
		}
	}
}
