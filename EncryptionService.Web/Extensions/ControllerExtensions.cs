using Microsoft.AspNetCore.Mvc;

namespace EncryptionService.Web.Extensions
{
	public static class ControllerExtensions
	{
		public static bool ValidateRequiredInput(this Controller controller,
			string? input, string modelPropertyName, string fieldDisplayName)
		{
			if (string.IsNullOrWhiteSpace(input))
			{
				controller.ModelState.AddModelError(modelPropertyName,
					$"{fieldDisplayName} is required.");
				return false;
			}
			return true;
		}
	}
}