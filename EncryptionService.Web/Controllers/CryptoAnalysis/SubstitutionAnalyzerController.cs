using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models.TranspositionCiphers.BlockTransposition;
using EncryptionService.Core.Models;
using EncryptionService.Web.Configurations;
using EncryptionService.Web.Extensions;
using EncryptionService.Web.Models.EncryptionViewModels;

namespace EncryptionService.Web.Controllers.CryptoAnalysis
{
	public class SubstitutionAnalyzerController(
		IEncryptionService<EncryptionResult, BlockTranspositionKey, int[]> encryptionService,
		IOptions<EncryptionSettings> encryptionSettings)
		: Controller
	{
		readonly IEncryptionService<EncryptionResult, BlockTranspositionKey, int[]>
			_encryptionService = encryptionService;
		readonly EncryptionSettings _encryptionSettings = encryptionSettings.Value;

		public IActionResult Index() => View();

		[HttpPost]
		public IActionResult Index(EncryptionViewModel<EncryptionResult> encryptionViewModel,
			string actionType)
		{
			if (!ModelState.IsValid)
				return View(encryptionViewModel);

			BlockTranspositionKey key = _encryptionSettings.BlockTranspositionKey;
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
