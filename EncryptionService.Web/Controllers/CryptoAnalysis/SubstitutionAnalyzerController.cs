using Microsoft.AspNetCore.Mvc;

using EncryptionService.Core.Interfaces;
using EncryptionService.Web.Extensions;
using EncryptionService.Web.Models.EncryptionViewModels;
using EncryptionService.Core.Models.CryptoAnalysis.SubstitutionAnalyzer;

namespace EncryptionService.Web.Controllers.CryptoAnalysis
{
	public class SubstitutionAnalyzerController(
		IEncryptionService<SubstitutionAnalyzerResult, SubstitutionAnalyzerKey,
			Dictionary<char, char>> cryptoAnalyzerService)
		: Controller
	{
		readonly IEncryptionService<SubstitutionAnalyzerResult, SubstitutionAnalyzerKey,
			Dictionary<char, char>> _cryptoAnalyzerService = cryptoAnalyzerService;

		public IActionResult Index() => View();

		[HttpPost]
		public async Task<IActionResult> Encrypt(
			FileEncryptionViewModel<SubstitutionAnalyzerResult> model)
		{
			SubstitutionAnalyzerResult encryptionResult;
			SubstitutionAnalyzerKey key = SubstitutionAnalyzerKey.UniqueInstance;

			if (model.EncryptionInputFile != null && model.EncryptionInputFile.Length > 0)
			{
				string text = await this.ReadFileAsync(model.EncryptionInputFile);
				encryptionResult = _cryptoAnalyzerService.Encrypt(text, key);
			}
			else
			{
				if (!ModelState.IsValid)
					return View("Index", model);

				encryptionResult = _cryptoAnalyzerService.Encrypt(model.InputText!, key);
			}

			model.EncryptionResult = encryptionResult;
			return View("Index", model);
		}

		[HttpPost]
		public async Task<IActionResult> Decrypt(
			FileEncryptionViewModel<SubstitutionAnalyzerResult> model)
		{
			SubstitutionAnalyzerResult encryptionResult;
			SubstitutionAnalyzerKey key = SubstitutionAnalyzerKey.UniqueInstance;

			if (model.DecryptionInputFile != null && model.DecryptionInputFile.Length > 0)
			{
				string text = await this.ReadFileAsync(model.DecryptionInputFile);
				encryptionResult = _cryptoAnalyzerService.Decrypt(text, key);
			}
			else
			{
				if (!ModelState.IsValid)
					return View("Index", model);

				encryptionResult = _cryptoAnalyzerService.Decrypt(model.EncryptedInputText!, key);
			}

			model.DecryptionResult = encryptionResult;
			return View("Index", model);
		}
	}
}