using EncryptionService.Configurations;
using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models;
using EncryptionService.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace EncryptionService.Controllers
{
	public class BlockTranspositionController(IEncryptionService<BlockTranspositionKey, int[]> encryptionService,
		IOptions<EncryptionSettings> encryptionSettings)
		: Controller
	{
		readonly IEncryptionService<BlockTranspositionKey, int[]> _encryptionService = encryptionService;
		readonly EncryptionSettings _encryptionSettings = encryptionSettings.Value;

		public IActionResult Index() => View();

		[HttpPost]
		public IActionResult Index(EncryptionViewModel encryptionViewModel, string actionType)
		{
			if (!ModelState.IsValid)
				return View(encryptionViewModel);

			BlockTranspositionKey key = _encryptionSettings.BlockTranspositionKey;
			EncryptionResult encryptionResult;

			if (actionType == "Encrypt")
			{
				encryptionResult = _encryptionService.Encrypt(encryptionViewModel.InputText!, key);
				encryptionViewModel.EncryptedText = encryptionResult.Text;
			}
			else if (actionType == "Decrypt")
			{
				encryptionResult = _encryptionService.Decrypt(encryptionViewModel.EncryptedInputText!, key);
				encryptionViewModel.DecryptedText = encryptionResult.Text;
			}

			return View(encryptionViewModel);
		}
	}
}