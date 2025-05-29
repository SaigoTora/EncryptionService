using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using EncryptionService.Web.Configurations;
using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models.TranspositionCiphers.EquivalentTransposition;
using EncryptionService.Web.Models.EncryptionViewModels;

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
		public IActionResult Encrypt(
			EncryptionViewModel<EquivalentTranspositionEncryptionResult> model)
		{
			if (!ModelState.IsValid)
				return View("Index", model);

			EquivalentTranspositionKey key = _encryptionSettings.EquivalentTranspositionKey;
			ViewData["KeyRowNumbers"] = key.Key.RowNumbers;
			ViewData["KeyColumnNumbers"] = key.Key.ColumnNumbers;
			int maxTextLength = key.Key.RowNumbers.Length * key.Key.ColumnNumbers.Length;

			if (!IsInputTextValid(model.InputText, nameof(model.InputText), maxTextLength))
				return View("Index", model);

			model.EncryptionResult = _encryptionService.Encrypt(model.InputText!, key);
			return View("Index", model);
		}

		[HttpPost]
		public IActionResult Decrypt(
			EncryptionViewModel<EquivalentTranspositionEncryptionResult> model)
		{
			if (!ModelState.IsValid)
				return View("Index", model);

			EquivalentTranspositionKey key = _encryptionSettings.EquivalentTranspositionKey;
			ViewData["KeyRowNumbers"] = key.Key.RowNumbers;
			ViewData["KeyColumnNumbers"] = key.Key.ColumnNumbers;
			int maxTextLength = key.Key.RowNumbers.Length * key.Key.ColumnNumbers.Length;

			if (!IsInputTextValid(model.EncryptedInputText, nameof(model.EncryptedInputText),
				maxTextLength))
				return View("Index", model);

			model.DecryptionResult = _encryptionService.Decrypt(model.EncryptedInputText!, key);
			return View("Index", model);
		}

		private bool IsInputTextValid(string text, string fieldName, int maxTextLength)
		{
			if (text.Length > maxTextLength)
			{
				ModelState.AddModelError(fieldName,
					"The length of the input text must be less than or equal to " +
					$"{maxTextLength}. You have entered characters: " +
					$"{text.Length}.");
				return false;
			}

			return true;
		}
	}
}