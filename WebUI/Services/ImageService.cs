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

		public static string SaveImageLocal(IFormFileCollection files)
		{
			if (RootPass != null)
			{
				IFormFile file = files.FirstOrDefault();

				string name = Guid.NewGuid().ToString();
				string extension = Path.GetExtension(file.FileName);
				string path = Path.Combine(RootPass + ENV.ImagePath, name + extension);

				using (var fileStream = File.Create(path))
				{
					file.CopyTo(fileStream);
				}

				return name + extension;
			}
			return null;
		}

		public static string SaveImageLocal(IFormFile file, string extension = null)
		{
			if (RootPass != null)
			{
				string name = Guid.NewGuid().ToString();
				if (String.IsNullOrEmpty(extension))
					extension = Path.GetExtension(file.FileName);

				string path = Path.Combine(RootPass + ENV.ImagePath, name + extension);

				using (var fileStream = File.Create(path))
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

		public static string UploadImageToServer(IFormFile formFile, string extension = null)
		{
			string name = Guid.NewGuid().ToString();
			if (String.IsNullOrEmpty(extension))
				extension = Path.GetExtension(formFile.FileName);

			Uri serverUri = new Uri(Path.Combine(RootPass + ENV.UploadPath, name + extension));

			if ((serverUri.Scheme != Uri.UriSchemeFtp) || (formFile.Length > 3000000))
				return null;

			try
			{
				FtpWebRequest request = (FtpWebRequest)WebRequest.Create(serverUri);
				request.Credentials = new NetworkCredential(ENV.FTPLogin, ENV.FTPPass);
				request.Method = WebRequestMethods.Ftp.UploadFile;

				byte[] fileContents;
				using (var ms = new MemoryStream())
				{
					formFile.CopyTo(ms);
					fileContents = ms.ToArray();
				}

				request.ContentLength = fileContents.Length;

				using (Stream requestStream = request.GetRequestStream())
				{
					requestStream.Write(fileContents, 0, fileContents.Length);
				}

				using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
				{
					return response.StatusCode == FtpStatusCode.ClosingData ? name + extension : null;
				}
			}
			catch (Exception)
			{
				return null;
			}
		}

		public static bool RemoveImageFromServer(string name)
		{
			try
			{
				Uri serverUri = new Uri(Path.Combine(RootPass + ENV.UploadPath, name));

				if (serverUri.Scheme != Uri.UriSchemeFtp)
					return false;

				FtpWebRequest request = (FtpWebRequest)WebRequest.Create(serverUri);
				request.Credentials = new NetworkCredential(ENV.FTPLogin, ENV.FTPPass);
				request.Method = WebRequestMethods.Ftp.DeleteFile;

				using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
				{
					response.Close();
					return true;
				}
			}
			catch (WebException ex)
			{
				if (((FtpWebResponse)ex.Response).StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
					return true;
				return false;
			}
		}
	}
}
