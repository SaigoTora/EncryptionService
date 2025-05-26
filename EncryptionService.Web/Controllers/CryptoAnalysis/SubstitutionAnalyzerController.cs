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
		public async Task<IActionResult> Index(
			FileEncryptionViewModel<SubstitutionAnalyzerResult> encryptionViewModel,
			string actionType)
		{
			if (!ModelState.IsValid)
				return View(encryptionViewModel);

			SubstitutionAnalyzerKey key = new();

			if (actionType == "Encrypt")
				return await ProcessEncrypt(encryptionViewModel, key);
			else if (actionType == "Decrypt")
				return await ProcessDecrypt(encryptionViewModel, key);

			return View(encryptionViewModel);
		}
		private async Task<ViewResult> ProcessEncrypt(
			FileEncryptionViewModel<SubstitutionAnalyzerResult> model, SubstitutionAnalyzerKey key)
		{
			SubstitutionAnalyzerResult encryptionResult;

			if (model.EncryptionInputFile != null
				&& model.EncryptionInputFile.Length > 0)
			{
				string text;

				using var reader = new StreamReader(model
					.EncryptionInputFile.OpenReadStream());
				text = await reader.ReadToEndAsync();
				encryptionResult = _cryptoAnalyzerService.Encrypt(text, key);
			}
			else
			{
				if (!this.ValidateRequiredInput(model.InputText,
					nameof(model.InputText), "Text"))
					return View(model);

				encryptionResult = _cryptoAnalyzerService.Encrypt(model.InputText!, key);
			}

			model.EncryptionResult = encryptionResult;
			return View(model);
		}
		private async Task<ViewResult> ProcessDecrypt(
			FileEncryptionViewModel<SubstitutionAnalyzerResult> model, SubstitutionAnalyzerKey key)
		{
			SubstitutionAnalyzerResult encryptionResult;

			if (model.DecryptionInputFile != null
				&& model.DecryptionInputFile.Length > 0)
			{
				string text;

				using var reader = new StreamReader(model
					.DecryptionInputFile.OpenReadStream());
				text = await reader.ReadToEndAsync();
				encryptionResult = _cryptoAnalyzerService.Decrypt(text, key);
			}
			else
			{
				if (!this.ValidateRequiredInput(model.EncryptedInputText,
					nameof(model.EncryptedInputText), "Encrypted text"))
					return View(model);

				encryptionResult = _cryptoAnalyzerService.Decrypt(
					model.EncryptedInputText!, key);
			}

			model.DecryptionResult = encryptionResult;
			return View(model);
		}
	}
}