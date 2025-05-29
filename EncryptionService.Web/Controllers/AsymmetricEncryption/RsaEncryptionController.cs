using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using EncryptionService.Core.Interfaces;
using EncryptionService.Web.Configurations;
using EncryptionService.Web.Extensions;
using EncryptionService.Web.Models.EncryptionViewModels;
using EncryptionService.Core.Models.AsymmetricEncryption.RsaEncryption;
using EncryptionService.Core.Services.AsymmetricEncryption;

namespace EncryptionService.Web.Controllers.AsymmetricEncryption
{
	public class RsaEncryptionController(
		IEncryptionService<RsaEncryptionResult, RsaEncryptionKey,
			RsaEncryptionKeyData> encryptionService,
		IOptions<EncryptionSettings> encryptionSettings)
		: Controller
	{
		readonly IEncryptionService<RsaEncryptionResult, RsaEncryptionKey, RsaEncryptionKeyData>
			_encryptionService = encryptionService;
		readonly EncryptionSettings _encryptionSettings = encryptionSettings.Value;

		public IActionResult Index() => View();

		[HttpPost]
		public async Task<IActionResult> Encrypt(FileEncryptionViewModel<RsaEncryptionResult> model)
		{
			RsaEncryptionResult encryptionResult;
			RsaEncryptionKey key = _encryptionSettings.RsaEncryptionKey;

			if (model.EncryptionInputFile != null && model.EncryptionInputFile.Length > 0)
			{
				string text = await this.ReadFileAsync(model.EncryptionInputFile);
				encryptionResult = _encryptionService.Encrypt(text, key);
			}
			else
			{
				if (!ModelState.IsValid)
					return View("Index", model);

				encryptionResult = _encryptionService.Encrypt(model.InputText!, key);
			}

			model.EncryptionResult = encryptionResult;
			return View("Index", model);
		}

		[HttpPost]
		public async Task<IActionResult> Decrypt(FileEncryptionViewModel<RsaEncryptionResult> model)
		{
			RsaEncryptionResult encryptionResult;
			RsaEncryptionKey key = _encryptionSettings.RsaEncryptionKey;

			if (model.DecryptionInputFile != null && model.DecryptionInputFile.Length > 0)
			{
				string text = await this.ReadFileAsync(model.DecryptionInputFile);

				model.EncryptedInputText = text;
				if (!IsEncryptedTextValid(text, nameof(model.DecryptionInputFile)))
					return View("Index", model);
				encryptionResult = _encryptionService.Decrypt(text, key);
			}
			else
			{
				if (!ModelState.IsValid)
					return View("Index", model);

				if (!IsEncryptedTextValid(model.EncryptedInputText,
					nameof(model.EncryptedInputText)))
					return View("Index", model);

				encryptionResult = _encryptionService.Decrypt(model.EncryptedInputText!, key);
			}

			model.DecryptionResult = encryptionResult;
			return View("Index", model);
		}

		private bool IsEncryptedTextValid(string text, string fieldName)
		{
			foreach (char ch in text ?? string.Empty)
				if (!char.IsDigit(ch) && !RsaEncryptionService.SEPARATOR.Contains(ch))
				{
					ModelState.AddModelError(fieldName,
						$"The encrypted input text can only contain digits and " +
						$"the separator '{RsaEncryptionService.SEPARATOR}' symbol.");
					return false;
				}

			return true;
		}
	}
}