using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models;
using EncryptionService.Core.Models.StreamCiphersAndGenerators.XorEncryption;
using EncryptionService.Web.Configurations;
using EncryptionService.Web.Models.EncryptionViewModels;

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
		public IActionResult Encrypt(EncryptionViewModel<EncryptionResult> model)
		{
			if (!ModelState.IsValid)
				return View("Index", model);

			XorEncryptionKey key = _encryptionSettings.XorEncryptionKey;

			model.EncryptionResult = _encryptionService.Encrypt(model.InputText!, key);
			return View("Index", model);
		}

		[HttpPost]
		public IActionResult Decrypt(EncryptionViewModel<EncryptionResult> model)
		{
			if (!ModelState.IsValid)
				return View("Index", model);

			XorEncryptionKey key = _encryptionSettings.XorEncryptionKey;

			model.DecryptionResult = _encryptionService.Decrypt(model.EncryptedInputText!, key);
			return View("Index", model);
		}
	}
}