using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using EncryptionService.Web.Configurations;
using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models.SubstitutionCiphers.PlayfairEncryption;
using EncryptionService.Web.Models.EncryptionViewModels;

namespace EncryptionService.Web.Controllers.SubstitutionCiphers
{
	public class PlayfairEncryptionController(
		IEncryptionService<PlayfairEncryptionResult, PlayfairEncryptionKey,
			string> encryptionService, IOptions<EncryptionSettings> encryptionSettings)
		: Controller
	{
		readonly IEncryptionService<PlayfairEncryptionResult, PlayfairEncryptionKey, string>
			_encryptionService = encryptionService;
		readonly EncryptionSettings _encryptionSettings = encryptionSettings.Value;

		public IActionResult Index() => View();

		[HttpPost]
		public IActionResult Encrypt(EncryptionViewModel<PlayfairEncryptionResult> model)
		{
			if (!ModelState.IsValid)
				return View("Index", model);

			PlayfairEncryptionKey key = _encryptionSettings.PlayfairEncryptionKey;

			model.EncryptionResult = _encryptionService.Encrypt(model.InputText!, key);
			return View("Index", model);
		}

		[HttpPost]
		public IActionResult Decrypt(EncryptionViewModel<PlayfairEncryptionResult> model)
		{
			if (!ModelState.IsValid)
				return View("Index", model);

			PlayfairEncryptionKey key = _encryptionSettings.PlayfairEncryptionKey;

			model.DecryptionResult = _encryptionService.Decrypt(model.EncryptedInputText!, key);
			return View("Index", model);
		}
	}
}