using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using EncryptionService.Web.Configurations;
using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models.TranspositionCiphers.VerticalTransposition;
using EncryptionService.Web.Models.EncryptionViewModels;

namespace EncryptionService.Web.Controllers.TranspositionCiphers
{
	public class VerticalTranspositionController(
		IEncryptionService<VerticalTranspositionEncryptionResult, VerticalTranspositionKey, string>
		encryptionService,
		IOptions<EncryptionSettings> encryptionSettings) : Controller
	{
		readonly IEncryptionService<VerticalTranspositionEncryptionResult,
			VerticalTranspositionKey, string> _encryptionService = encryptionService;
		readonly EncryptionSettings _encryptionSettings = encryptionSettings.Value;

		public IActionResult Index() => View();

		[HttpPost]
		public IActionResult Encrypt(
			EncryptionViewModel<VerticalTranspositionEncryptionResult> model)
		{
			if (!ModelState.IsValid)
				return View("Index", model);

			VerticalTranspositionKey key = _encryptionSettings.VerticalTranspositionKey;
			ViewData["Key"] = key.Key;

			model.EncryptionResult = _encryptionService.Encrypt(model.InputText!, key);
			return View("Index", model);
		}

		[HttpPost]
		public IActionResult Decrypt(
			EncryptionViewModel<VerticalTranspositionEncryptionResult> model)
		{
			if (!ModelState.IsValid)
				return View("Index", model);

			VerticalTranspositionKey key = _encryptionSettings.VerticalTranspositionKey;
			ViewData["Key"] = key.Key;

			model.DecryptionResult = _encryptionService.Decrypt(model.EncryptedInputText!, key);
			return View("Index", model);
		}
	}
}