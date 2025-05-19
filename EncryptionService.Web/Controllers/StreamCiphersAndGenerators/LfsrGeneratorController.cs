using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models.StreamCiphersAndGenerators.LfsrGenerator;
using EncryptionService.Web.Configurations;
using EncryptionService.Web.Models.EncryptionViewModels;
using EncryptionService.Web.Extensions;

namespace EncryptionService.Web.Controllers.StreamCiphersAndGenerators
{
	public class LfsrGeneratorController(
		IEncryptionService<LfsrEncryptionResult, LfsrEncryptionKey, int[]> encryptionService,
		IOptions<EncryptionSettings> encryptionSettings)
		: Controller
	{
		readonly IEncryptionService<LfsrEncryptionResult, LfsrEncryptionKey,
			int[]> _encryptionService = encryptionService;
		readonly EncryptionSettings _encryptionSettings = encryptionSettings.Value;

		public IActionResult Index() => View();

		[HttpPost]
		public async Task<IActionResult> Index(
			LfsrEncryptionViewModel<LfsrEncryptionResult> encryptionViewModel, string actionType)
		{
			if (!ModelState.IsValid)
				return View(encryptionViewModel);

			LfsrEncryptionKey key = _encryptionSettings.LfsrGeneratorKey;
			key.SetInitialState(encryptionViewModel.InitialState);

			if (actionType == "Encrypt")
				return await ProcessEncrypt(encryptionViewModel, key);
			else if (actionType == "Decrypt")
				return await ProcessDecrypt(encryptionViewModel, key);

			return View(encryptionViewModel);
		}

		private async Task<ViewResult> ProcessEncrypt(
			LfsrEncryptionViewModel<LfsrEncryptionResult> model, LfsrEncryptionKey key)
		{
			LfsrEncryptionResult encryptionResult;
			key.SetEncryptionFormat(model.EncryptionFormat);

			if (model.EncryptionInputFile != null
				&& model.EncryptionInputFile.Length > 0)
			{
				string text;

				using var reader = new StreamReader(model
					.EncryptionInputFile.OpenReadStream());
				text = await reader.ReadToEndAsync();
				encryptionResult = _encryptionService.Encrypt(text, key);
			}
			else
			{
				if (!this.ValidateRequiredInput(model.InputText,
					nameof(model.InputText), "Text"))
					return View(model);

				encryptionResult = _encryptionService.Encrypt(model.InputText!,
					key);
			}
			model.EncryptionResult = encryptionResult;

			return View(model);
		}
		private async Task<ViewResult> ProcessDecrypt(
			LfsrEncryptionViewModel<LfsrEncryptionResult> model, LfsrEncryptionKey key)
		{
			LfsrEncryptionResult encryptionResult;
			key.SetEncryptionFormat(model.DecryptionFormat);

			if (model.DecryptionInputFile != null
				&& model.DecryptionInputFile.Length > 0)
			{
				string text;

				using var reader = new StreamReader(model
					.DecryptionInputFile.OpenReadStream());
				text = await reader.ReadToEndAsync();
				encryptionResult = _encryptionService.Decrypt(text, key);
			}
			else
			{
				if (!this.ValidateRequiredInput(model.EncryptedInputText,
					nameof(model.EncryptedInputText), "Encrypted text"))
					return View(model);

				encryptionResult = _encryptionService.Decrypt(
					model.EncryptedInputText!, key);
			}
			model.DecryptionResult = encryptionResult;

			return View(model);
		}
	}
}