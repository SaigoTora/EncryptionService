using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models.AsymmetricEncryption.KnapsackEncryption;
using EncryptionService.Web.Configurations;
using EncryptionService.Web.Extensions;
using EncryptionService.Web.Models.EncryptionViewModels;
using EncryptionService.Core.Services.AsymmetricEncryption;

namespace EncryptionService.Web.Controllers.AsymmetricEncryption
{
	public class KnapsackEncryptionController(
		IEncryptionService<KnapsackEncryptionResult, KnapsackEncryptionKey,
		KnapsackEncryptionKeyData> encryptionService,
		IOptions<EncryptionSettings> encryptionSettings)
		: Controller
	{
		readonly IEncryptionService<KnapsackEncryptionResult, KnapsackEncryptionKey,
		KnapsackEncryptionKeyData>
			_encryptionService = encryptionService;
		readonly EncryptionSettings _encryptionSettings = encryptionSettings.Value;

		public IActionResult Index() => View();

		[HttpPost]
		public IActionResult Index(EncryptionViewModel<KnapsackEncryptionResult> encryptionViewModel,
			string actionType)
		{
			if (!ModelState.IsValid)
				return View(encryptionViewModel);

			KnapsackEncryptionKey key = _encryptionSettings.KnapsackEncryptionKey;

			if (actionType == "Encrypt")
				ProcessEncrypt(encryptionViewModel, key);
			else if (actionType == "Decrypt")
				ProcessDecrypt(encryptionViewModel, key);

			return View(encryptionViewModel);
		}
		private ViewResult ProcessEncrypt(
			EncryptionViewModel<KnapsackEncryptionResult> model,
			KnapsackEncryptionKey key)
		{
			if (!this.ValidateRequiredInput(model.InputText, nameof(model.InputText), "Text"))
				return View(model);

			model.EncryptionResult = _encryptionService.Encrypt(model.InputText!, key);
			return View(model);
		}
		private ViewResult ProcessDecrypt(
			EncryptionViewModel<KnapsackEncryptionResult> model,
			KnapsackEncryptionKey key)
		{
			if (!this.ValidateRequiredInput(model.EncryptedInputText,
				nameof(model.EncryptedInputText), "Encrypted text"))
				return View(model);

			foreach (char ch in model.EncryptedInputText ?? string.Empty)
				if (!char.IsDigit(ch) && !KnapsackEncryptionService.SEPARATOR.Contains(ch))
				{
					ModelState.AddModelError(nameof(model.EncryptedInputText),
						$"The encrypted input text can only contain digits and " +
						$"the separator({KnapsackEncryptionService.SEPARATOR}) symbol.");
					return View(model);
				}

			model.DecryptionResult = _encryptionService.Decrypt(model.EncryptedInputText!, key);
			return View(model);
		}
	}
}