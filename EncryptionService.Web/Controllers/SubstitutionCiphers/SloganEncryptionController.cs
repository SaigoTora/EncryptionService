using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using EncryptionService.Web.Configurations;
using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models.SubstitutionCiphers.SloganEncryption;
using EncryptionService.Web.Models.EncryptionViewModels;

namespace EncryptionService.Web.Controllers.SubstitutionCiphers
{
	public class SloganEncryptionController(
		IEncryptionService<SloganEncryptionResult, SloganEncryptionKey, string> encryptionService,
		IOptions<EncryptionSettings> encryptionSettings)
		: Controller
	{
		readonly IEncryptionService<SloganEncryptionResult, SloganEncryptionKey, string>
			_encryptionService = encryptionService;
		readonly EncryptionSettings _encryptionSettings = encryptionSettings.Value;

		public IActionResult Index() => View();

		[HttpPost]
		public IActionResult Encrypt(EncryptionViewModel<SloganEncryptionResult> model)
		{
			if (!ModelState.IsValid)
				return View("Index", model);

			SloganEncryptionKey key = _encryptionSettings.SloganEncryptionKey;

			model.EncryptionResult = _encryptionService.Encrypt(model.InputText!, key); ;
			return View("Index", model);
		}

		[HttpPost]
		public IActionResult Decrypt(EncryptionViewModel<SloganEncryptionResult> model)
		{
			if (!ModelState.IsValid)
				return View("Index", model);

			SloganEncryptionKey key = _encryptionSettings.SloganEncryptionKey;

			model.DecryptionResult = _encryptionService.Decrypt(model.EncryptedInputText!, key);
			return View("Index", model);
		}
	}
}