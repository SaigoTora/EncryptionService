using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using EncryptionService.Configurations;
using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models;
using EncryptionService.Models;
using EncryptionService.Core.Models.XorEncryption;

namespace EncryptionService.Controllers
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
				encryptionResult = _encryptionService.Encrypt(encryptionViewModel.InputText!, key);
				encryptionViewModel.EncryptionResult = encryptionResult;
			}
			else if (actionType == "Decrypt")
			{
				encryptionResult = _encryptionService.Decrypt(
					encryptionViewModel.EncryptedInputText!, key);
				encryptionViewModel.DecryptionResult = encryptionResult;
			}

			return View(encryptionViewModel);
		}
	}
}