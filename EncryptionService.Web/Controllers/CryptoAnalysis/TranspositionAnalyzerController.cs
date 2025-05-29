using Microsoft.AspNetCore.Mvc;

using EncryptionService.Core.Interfaces;
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
		public IActionResult Encrypt(EncryptionViewModel<TranspositionAnalyzerResult> model)
		{
			if (!ModelState.IsValid)
				return View("Index", model);

			TranspositionAnalyzerKey key = new(model.InputText!.Length);
			model.EncryptionResult = _cryptoAnalyzerService.Encrypt(model.InputText!, key);

			return View("Index", model);
		}

		[HttpPost]
		public IActionResult Decrypt(EncryptionViewModel<TranspositionAnalyzerResult> model)
		{
			if (!ModelState.IsValid)
				return View("Index", model);

			TranspositionAnalyzerKey key = new(model.EncryptedInputText!.Length);
			model.DecryptionResult = _cryptoAnalyzerService.Decrypt(model.EncryptedInputText!, key);

			return View("Index", model);
		}
	}
}