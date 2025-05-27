using Microsoft.AspNetCore.Mvc;

using EncryptionService.Core.Interfaces;
using EncryptionService.Web.Extensions;
using EncryptionService.Web.Models.EncryptionViewModels;
using EncryptionService.Core.Models.CryptoAnalysis.TranspositionAnalyzer;

namespace EncryptionService.Web.Controllers.CryptoAnalysis
{
	public class TranspositionAnalyzerController(
		IEncryptionService<TranspositionAnalyzerResult, TranspositionAnalyzerKey,
			HashSet<int>> cryptoAnalyzerService)
		: Controller
	{
		readonly IEncryptionService<TranspositionAnalyzerResult, TranspositionAnalyzerKey,
			HashSet<int>> _cryptoAnalyzerService = cryptoAnalyzerService;

		public IActionResult Index() => View();

		[HttpPost]
		public IActionResult Index(
			EncryptionViewModel<TranspositionAnalyzerResult> encryptionViewModel,
			string actionType)
		{
			if (!ModelState.IsValid)
				return View(encryptionViewModel);

			TranspositionAnalyzerResult encryptionResult;

			if (actionType == "Encrypt")
			{
				if (!this.ValidateRequiredInput(encryptionViewModel.InputText,
					nameof(encryptionViewModel.InputText), "Text"))
					return View(encryptionViewModel);

				TranspositionAnalyzerKey key = new(encryptionViewModel.InputText!.Length);
				encryptionResult = _cryptoAnalyzerService.Encrypt(encryptionViewModel.InputText!,
					key);
				encryptionViewModel.EncryptionResult = encryptionResult;
			}
			else if (actionType == "Decrypt")
			{
				if (!this.ValidateRequiredInput(encryptionViewModel.EncryptedInputText,
					nameof(encryptionViewModel.EncryptedInputText), "Encrypted text"))
					return View(encryptionViewModel);

				TranspositionAnalyzerKey key = new(encryptionViewModel.EncryptedInputText!.Length);
				encryptionResult = _cryptoAnalyzerService.Decrypt(
					encryptionViewModel.EncryptedInputText!, key);
				encryptionViewModel.DecryptionResult = encryptionResult;
			}

			return View(encryptionViewModel);
		}
	}
}