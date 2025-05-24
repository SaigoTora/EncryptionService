using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using EncryptionService.Core.Interfaces;
using EncryptionService.Web.Configurations;
using EncryptionService.Web.Extensions;
using EncryptionService.Web.Models.EncryptionViewModels;
using EncryptionService.Core.Models.AsymmetricEncryption.ElGamalEncryption;
using EncryptionService.Core.Services.AsymmetricEncryption;

namespace EncryptionService.Web.Controllers.AsymmetricEncryption
{
	public class ElGamalEncryptionController(
		IEncryptionService<ElGamalEncryptionResult, ElGamalEncryptionKey,
			ElGamalEncryptionKeyData> encryptionService,
		IOptions<EncryptionSettings> encryptionSettings)
		: Controller
	{
		readonly IEncryptionService<ElGamalEncryptionResult, ElGamalEncryptionKey,
			ElGamalEncryptionKeyData> _encryptionService = encryptionService;
		readonly EncryptionSettings _encryptionSettings = encryptionSettings.Value;

		public IActionResult Index() => View();

		[HttpPost]
		public async Task<IActionResult> Index(
			FileEncryptionViewModel<ElGamalEncryptionResult> encryptionViewModel,
			string actionType)
		{
			if (!ModelState.IsValid)
				return View(encryptionViewModel);

			ElGamalEncryptionKey key = _encryptionSettings.ElGamalEncryptionKey;

			if (actionType == "Encrypt")
				await ProcessEncrypt(encryptionViewModel, key);
			else if (actionType == "Decrypt")
				await ProcessDecrypt(encryptionViewModel, key);

			return View(encryptionViewModel);
		}
		private async Task<ViewResult> ProcessEncrypt(
			FileEncryptionViewModel<ElGamalEncryptionResult> model,
			ElGamalEncryptionKey key)
		{
			ElGamalEncryptionResult encryptionResult;
			if (model.EncryptionInputFile != null
				&& model.EncryptionInputFile.Length > 0)
			{
				string text;

				using var reader = new StreamReader(model
					.EncryptionInputFile.OpenReadStream());
				text = await reader.ReadToEndAsync();
				encryptionResult = _encryptionService.Encrypt(text, key);
			}
			else
			{
				if (!this.ValidateRequiredInput(model.InputText,
					nameof(model.InputText), "Text"))
					return View(model);

				encryptionResult = _encryptionService.Encrypt(model.InputText!, key);
			}

			model.EncryptionResult = encryptionResult;
			return View(model);
		}
		private async Task<ViewResult> ProcessDecrypt(
			FileEncryptionViewModel<ElGamalEncryptionResult> model,
			ElGamalEncryptionKey key)
		{
			ElGamalEncryptionResult encryptionResult;
			if (model.DecryptionInputFile != null
				&& model.DecryptionInputFile.Length > 0)
			{
				string text;

				using var reader = new StreamReader(model
					.DecryptionInputFile.OpenReadStream());
				text = await reader.ReadToEndAsync();

				model.EncryptedInputText = text;
				if (!IsEncryptedTextValid(model))
					return View(model);
				encryptionResult = _encryptionService.Decrypt(text, key);
			}
			else
			{
				if (!this.ValidateRequiredInput(model.EncryptedInputText,
					nameof(model.EncryptedInputText), "Encrypted text"))
					return View(model);

				if (!IsEncryptedTextValid(model))
					return View(model);

				encryptionResult = _encryptionService.Decrypt(
					model.EncryptedInputText!, key);
			}

			model.DecryptionResult = encryptionResult;
			return View(model);
		}
		private bool IsEncryptedTextValid(FileEncryptionViewModel<ElGamalEncryptionResult> model)
		{
			foreach (char ch in model.EncryptedInputText ?? string.Empty)
				if (!char.IsDigit(ch) && ElGamalEncryptionService.SEPARATOR_A != ch
					&& ElGamalEncryptionService.SEPARATOR_B != ch)
				{
					ModelState.AddModelError(nameof(model.EncryptedInputText),
						$"The encrypted input text can only contain digits and " +
						$"the separator '{KnapsackEncryptionService.SEPARATOR}' symbol.");
					return false;
				}

			return true;
		}
	}
}