using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using EncryptionService.Web.Configurations;
using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models;
using EncryptionService.Core.Models.TranspositionCiphers.BlockTransposition;
using EncryptionService.Web.Models.EncryptionViewModels;

namespace EncryptionService.Web.Controllers.TranspositionCiphers
{
	public class BlockTranspositionController(
		IEncryptionService<EncryptionResult, BlockTranspositionKey, int[]> encryptionService,
		IOptions<EncryptionSettings> encryptionSettings)
		: Controller
	{
		readonly IEncryptionService<EncryptionResult, BlockTranspositionKey, int[]>
			_encryptionService = encryptionService;
		readonly EncryptionSettings _encryptionSettings = encryptionSettings.Value;

		public IActionResult Index() => View();

		[HttpPost]
		public IActionResult Encrypt(EncryptionViewModel<EncryptionResult> model)
		{
			if (!ModelState.IsValid)
				return View("Index", model);

			BlockTranspositionKey key = _encryptionSettings.BlockTranspositionKey;

			model.EncryptionResult = _encryptionService.Encrypt(model.InputText!, key);
			return View("Index", model);
		}
		[HttpPost]
		public IActionResult Decrypt(EncryptionViewModel<EncryptionResult> model)
		{
			if (!ModelState.IsValid)
				return View("Index", model);

			BlockTranspositionKey key = _encryptionSettings.BlockTranspositionKey;

			model.DecryptionResult = _encryptionService.Decrypt(model.EncryptedInputText!, key);
			return View("Index", model);
		}
	}
}