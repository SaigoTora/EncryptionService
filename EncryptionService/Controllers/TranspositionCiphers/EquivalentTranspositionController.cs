using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using EncryptionService.Configurations;
using EncryptionService.Core.Interfaces;
using EncryptionService.Models;
using EncryptionService.Core.Models.TranspositionCiphers.EquivalentTransposition;

namespace EncryptionService.Controllers.TranspositionCiphers
{
	public class EquivalentTranspositionController(
		IEncryptionService<EquivalentTranspositionEncryptionResult, EquivalentTranspositionKey,
			EquivalentTranspositionKeyData> encryptionService,
		IOptions<EncryptionSettings> encryptionSettings)
		: Controller
	{
		readonly IEncryptionService<EquivalentTranspositionEncryptionResult,
			EquivalentTranspositionKey, EquivalentTranspositionKeyData> _encryptionService
			= encryptionService;
		readonly EncryptionSettings _encryptionSettings = encryptionSettings.Value;

		public IActionResult Index() => View();

		[HttpPost]
		public IActionResult Index(
			EncryptionViewModel<EquivalentTranspositionEncryptionResult> encryptionViewModel,
			string actionType)
		{
			if (!ModelState.IsValid)
				return View(encryptionViewModel);

			EquivalentTranspositionKey key = _encryptionSettings.EquivalentTranspositionKey;
			ViewData["KeyRowNumbers"] = key.Key.RowNumbers;
			ViewData["KeyColumnNumbers"] = key.Key.ColumnNumbers;
			EquivalentTranspositionEncryptionResult encryptionResult;
			int maxTextLength = key.Key.RowNumbers.Length * key.Key.ColumnNumbers.Length;

			if (actionType == "Encrypt")
			{
				if (encryptionViewModel.InputText!.Length > maxTextLength)
				{
					ModelState.AddModelError("InputText",
						"The length of the input text must be less than or equal to " +
						$"{maxTextLength}. You have entered characters: " +
						$"{encryptionViewModel.InputText.Length}.");
					return View(encryptionViewModel);
				}
				encryptionResult = _encryptionService.Encrypt(encryptionViewModel.InputText!, key);
				encryptionViewModel.EncryptionResult = encryptionResult;
			}
			else if (actionType == "Decrypt")
			{
				if (encryptionViewModel.EncryptedInputText!.Length > maxTextLength)
				{
					ModelState.AddModelError("EncryptedInputText",
						"The length of the encrypted input text must be less than or equal to " +
						$"{maxTextLength}. You have entered characters: " +
						$"{encryptionViewModel.EncryptedInputText.Length}.");
					return View(encryptionViewModel);
				}
				encryptionResult = _encryptionService.Decrypt(
					encryptionViewModel.EncryptedInputText!, key);
				encryptionViewModel.DecryptionResult = encryptionResult;
			}

			return View(encryptionViewModel);
		}
	}
}