using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using EncryptionService.Web.Configurations;
using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models.TranspositionCiphers.EquivalentTransposition;
using EncryptionService.Web.Models.EncryptionViewModels;
using EncryptionService.Web.Extensions;

namespace EncryptionService.Web.Controllers.TranspositionCiphers
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
			int maxTextLength = key.Key.RowNumbers.Length * key.Key.ColumnNumbers.Length;

			if (actionType == "Encrypt")
				return ProcessEncrypt(encryptionViewModel, key, maxTextLength);
			else if (actionType == "Decrypt")
				return ProcessDecrypt(encryptionViewModel, key, maxTextLength);

			return View(encryptionViewModel);
		}

		private ViewResult ProcessEncrypt(
			EncryptionViewModel<EquivalentTranspositionEncryptionResult> model,
			EquivalentTranspositionKey key, int maxTextLength)
		{
			if (!this.ValidateRequiredInput(model.InputText,
				nameof(model.InputText), "Text"))
				return View(model);

			if (model.InputText!.Length > maxTextLength)
			{
				ModelState.AddModelError(nameof(model.InputText),
					"The length of the input text must be less than or equal to " +
					$"{maxTextLength}. You have entered characters: " +
					$"{model.InputText.Length}.");
				return View(model);
			}

			model.EncryptionResult = _encryptionService.Encrypt(model.InputText!, key);
			return View(model);
		}
		private ViewResult ProcessDecrypt(
			EncryptionViewModel<EquivalentTranspositionEncryptionResult> model,
			EquivalentTranspositionKey key, int maxTextLength)
		{
			if (!this.ValidateRequiredInput(model.EncryptedInputText,
				nameof(model.EncryptedInputText), "Encrypted text"))
				return View(model);

			if (model.EncryptedInputText!.Length > maxTextLength)
			{
				ModelState.AddModelError(nameof(model.EncryptedInputText),
					"The length of the encrypted input text must be less than or equal to " +
					$"{maxTextLength}. You have entered characters: " +
					$"{model.EncryptedInputText.Length}.");
				return View(model);
			}
			model.DecryptionResult = _encryptionService.Decrypt(
				model.EncryptedInputText!, key);

			return View(model);
		}
	}
}