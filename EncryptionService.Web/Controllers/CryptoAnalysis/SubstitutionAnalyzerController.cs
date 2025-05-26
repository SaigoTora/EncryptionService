using Microsoft.AspNetCore.Mvc;

using EncryptionService.Core.Interfaces;
using EncryptionService.Web.Extensions;
using EncryptionService.Web.Models.EncryptionViewModels;
using EncryptionService.Core.Models.CryptoAnalysis.SubstitutionAnalyzer;

namespace EncryptionService.Web.Controllers.CryptoAnalysis
{
	public class SubstitutionAnalyzerController(
		ICryptoAnalyzer<SubstitutionAnalyzerResult, SubstitutionAnalyzerKey,
			SubstitutionAnalyzerKeyData> cryptoAnalyzerService)
		: Controller
	{
		readonly ICryptoAnalyzer<SubstitutionAnalyzerResult, SubstitutionAnalyzerKey,
			SubstitutionAnalyzerKeyData> _cryptoAnalyzerService = cryptoAnalyzerService;

		public IActionResult Index() => View();

		[HttpPost]
		public IActionResult Index(EncryptionViewModel<SubstitutionAnalyzerResult> encryptionViewModel,
			string actionType)
		{
			if (!ModelState.IsValid)
				return View(encryptionViewModel);

			SubstitutionAnalyzerKey key = new();
			SubstitutionAnalyzerResult encryptionResult;

			if (actionType == "Encrypt")
			{
				if (!this.ValidateRequiredInput(encryptionViewModel.InputText,
					nameof(encryptionViewModel.InputText), "Text"))
					return View(encryptionViewModel);

				encryptionResult = _cryptoAnalyzerService.Encrypt(encryptionViewModel.InputText!,
					key);
				encryptionViewModel.EncryptionResult = encryptionResult;
			}
			else if (actionType == "Decrypt")
			{
				if (!this.ValidateRequiredInput(encryptionViewModel.EncryptedInputText,
					nameof(encryptionViewModel.EncryptedInputText), "Encrypted text"))
					return View(encryptionViewModel);

				encryptionResult = _cryptoAnalyzerService.Decrypt(
					encryptionViewModel.EncryptedInputText!, key);
				encryptionViewModel.DecryptionResult = encryptionResult;
			}

			return View(encryptionViewModel);
		}
	}
}