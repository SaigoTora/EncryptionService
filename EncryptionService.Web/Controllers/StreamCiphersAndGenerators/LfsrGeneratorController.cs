using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models.StreamCiphersAndGenerators.LfsrGenerator;
using EncryptionService.Web.Configurations;
using EncryptionService.Web.Models.EncryptionViewModels;

namespace EncryptionService.Web.Controllers.StreamCiphersAndGenerators
{
	public class LfsrGeneratorController(
		IEncryptionService<LfsrEncryptionResult, LfsrEncryptionKey, int[]> encryptionService,
		IOptions<EncryptionSettings> encryptionSettings)
		: Controller
	{
		readonly IEncryptionService<LfsrEncryptionResult, LfsrEncryptionKey, int[]> _encryptionService
			= encryptionService;
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
			LfsrEncryptionResult encryptionResult;

			if (actionType == "Encrypt")
				encryptionResult = await ProcessEncrypt(encryptionViewModel, key);
			else if (actionType == "Decrypt")
				encryptionResult = await ProcessDecrypt(encryptionViewModel, key);

			return View(encryptionViewModel);
		}

		private async Task<LfsrEncryptionResult> ProcessEncrypt(
			LfsrEncryptionViewModel<LfsrEncryptionResult> encryptionViewModel,
			LfsrEncryptionKey key)
		{
			LfsrEncryptionResult encryptionResult;
			key.SetEncryptionFormat(encryptionViewModel.EncryptionFormat);

			if (encryptionViewModel.EncryptionInputFile != null
				&& encryptionViewModel.EncryptionInputFile.Length > 0)
			{
				string text;

				using var reader = new StreamReader(encryptionViewModel
					.EncryptionInputFile.OpenReadStream());
				text = await reader.ReadToEndAsync();
				encryptionResult = _encryptionService.Encrypt(text, key);
				encryptionViewModel.EncryptionResult = encryptionResult;
			}
			else
			{
				encryptionResult = _encryptionService.Encrypt(encryptionViewModel.InputText!,
					key);
				encryptionViewModel.EncryptionResult = encryptionResult;
			}

			return encryptionResult;
		}
		private async Task<LfsrEncryptionResult> ProcessDecrypt(
			LfsrEncryptionViewModel<LfsrEncryptionResult> encryptionViewModel,
			LfsrEncryptionKey key)
		{
			LfsrEncryptionResult encryptionResult;
			key.SetEncryptionFormat(encryptionViewModel.DecryptionFormat);

			if (encryptionViewModel.DecryptionInputFile != null
				&& encryptionViewModel.DecryptionInputFile.Length > 0)
			{
				string text;

				using var reader = new StreamReader(encryptionViewModel
					.DecryptionInputFile.OpenReadStream());
				text = await reader.ReadToEndAsync();
				encryptionResult = _encryptionService.Decrypt(text, key);
				encryptionViewModel.DecryptionResult = encryptionResult;
			}
			else
			{
				encryptionResult = _encryptionService.Decrypt(
					encryptionViewModel.EncryptedInputText!, key);
				encryptionViewModel.DecryptionResult = encryptionResult;
			}

			return encryptionResult;
		}
	}
}