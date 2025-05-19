using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models;
using EncryptionService.Core.Models.StreamCiphersAndGenerators.XorEncryption;
using EncryptionService.Web.Configurations;
using EncryptionService.Web.Models.EncryptionViewModels;
using EncryptionService.Web.Extensions;

namespace EncryptionService.Web.Controllers.StreamCiphersAndGenerators
{
	public class XorEncryptionController(
		IEncryptionService<EncryptionResult, XorEncryptionKey, string> encryptionService,
		IOptions<EncryptionSettings> encryptionSettings)
		: Controller
	{
		readonly IEncryptionService<EncryptionResult, XorEncryptionKey, string>
			_encryptionService = encryptionService;
		readonly EncryptionSettings _encryptionSettings = encryptionSettings.Value;

		public IActionResult Index() => View();

		[HttpPost]
		public IActionResult Index(EncryptionViewModel<EncryptionResult> encryptionViewModel,
			string actionType)
		{
			if (!ModelState.IsValid)
				return View(encryptionViewModel);

			XorEncryptionKey key = _encryptionSettings.XorEncryptionKey;
			EncryptionResult encryptionResult;

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