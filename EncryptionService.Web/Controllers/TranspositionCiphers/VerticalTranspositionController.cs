using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using EncryptionService.Web.Configurations;
using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models.TranspositionCiphers.VerticalTransposition;
using EncryptionService.Web.Models.EncryptionViewModels;
using EncryptionService.Web.Extensions;

namespace EncryptionService.Web.Controllers.TranspositionCiphers
{
	public class VerticalTranspositionController(
		IEncryptionService<VerticalTranspositionEncryptionResult, VerticalTranspositionKey, string>
		encryptionService,
		IOptions<EncryptionSettings> encryptionSettings) : Controller
	{
		readonly IEncryptionService<VerticalTranspositionEncryptionResult,
			VerticalTranspositionKey, string> _encryptionService = encryptionService;
		readonly EncryptionSettings _encryptionSettings = encryptionSettings.Value;

		public IActionResult Index() => View();

		[HttpPost]
		public IActionResult Index(
			EncryptionViewModel<VerticalTranspositionEncryptionResult> encryptionViewModel,
			string actionType)
		{
			if (!ModelState.IsValid)
				return View(encryptionViewModel);

			VerticalTranspositionKey key = _encryptionSettings.VerticalTranspositionKey;
			ViewData["Key"] = key.Key;
			VerticalTranspositionEncryptionResult encryptionResult;

			if (actionType == "Encrypt")
			{
				if (!this.ValidateRequiredInput(encryptionViewModel.InputText,
					nameof(encryptionViewModel.InputText), "Text"))
					return View(encryptionViewModel);

				encryptionResult = _encryptionService.Encrypt(encryptionViewModel.InputText!, key);
				encryptionViewModel.EncryptionResult = encryptionResult;
			}
			else if (actionType == "Decrypt")
			{
				if (!this.ValidateRequiredInput(encryptionViewModel.EncryptedInputText,
					nameof(encryptionViewModel.EncryptedInputText), "Encrypted text"))
					return View(encryptionViewModel);

				encryptionResult = _encryptionService.Decrypt(
					encryptionViewModel.EncryptedInputText!, key);
				encryptionViewModel.DecryptionResult = encryptionResult;
			}

			return View(encryptionViewModel);
		}
	}
}