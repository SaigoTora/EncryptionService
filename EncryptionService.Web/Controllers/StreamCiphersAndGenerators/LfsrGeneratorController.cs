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
		public async Task<IActionResult> Encrypt(
			LfsrEncryptionViewModel<LfsrEncryptionResult> model)
		{
			if (string.IsNullOrEmpty(model.EncryptionInitialState))
				return View("Index", model);

			LfsrEncryptionResult encryptionResult;
			LfsrEncryptionKey key = _encryptionSettings.LfsrEncryptionKey;
			key.SetInitialState(model.EncryptionInitialState);
			key.SetEncryptionFormat(model.EncryptionFormat);

			if (model.EncryptionInputFile != null && model.EncryptionInputFile.Length > 0)
			{
				string text = await this.ReadFileAsync(model.EncryptionInputFile);
				encryptionResult = _encryptionService.Encrypt(text, key);
			}
			else
			{
				if (!ModelState.IsValid)
					return View("Index", model);

				encryptionResult = _encryptionService.Encrypt(model.InputText!, key);
			}

			model.EncryptionResult = encryptionResult;
			return View("Index", model);
		}

		[HttpPost]
		public async Task<IActionResult> Decrypt(
			LfsrEncryptionViewModel<LfsrEncryptionResult> model)
		{
			if (string.IsNullOrEmpty(model.DecryptionInitialState))
				return View("Index", model);

			LfsrEncryptionResult encryptionResult;
			LfsrEncryptionKey key = _encryptionSettings.LfsrEncryptionKey;
			key.SetInitialState(model.DecryptionInitialState);
			key.SetEncryptionFormat(model.DecryptionFormat);

			if (model.DecryptionInputFile != null
				&& model.DecryptionInputFile.Length > 0)
			{
				string text = await this.ReadFileAsync(model.DecryptionInputFile);
				encryptionResult = _encryptionService.Decrypt(text, key);
			}
			else
			{
				if (!ModelState.IsValid)
					return View("Index", model);

				encryptionResult = _encryptionService.Decrypt(model.EncryptedInputText!, key);
			}

			model.DecryptionResult = encryptionResult;
			return View("Index", model);
		}
	}
}