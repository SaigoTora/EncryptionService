using Microsoft.AspNetCore.Mvc;

namespace EncryptionService.Web.Extensions
{
	public static class ControllerExtensions
	{
		public static async Task<string> ReadFileAsync(this Controller controller, IFormFile file)
		{
			using var reader = new StreamReader(file.OpenReadStream());
			return await reader.ReadToEndAsync();
		}
	}
}