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
		public IActionResult Index(LfsrGeneratorViewModel<LfsrEncryptionResult> encryptionViewModel,
			string actionType)
		{
			if (!ModelState.IsValid)
				return View(encryptionViewModel);

			LfsrEncryptionKey key = _encryptionSettings.LfsrGeneratorKey;
			key.SetInitialState(encryptionViewModel.InitialState);
			LfsrEncryptionResult encryptionResult;

			if (actionType == "Encrypt")
			{
				key.SetEncryptionFormat(encryptionViewModel.EncryptionFormat);
				encryptionResult = _encryptionService.Encrypt(encryptionViewModel.InputText!, key);
				encryptionViewModel.EncryptionResult = encryptionResult;
			}
			else if (actionType == "Decrypt")
			{
				key.SetEncryptionFormat(encryptionViewModel.DecryptionFormat);
				encryptionResult = _encryptionService.Decrypt(
					encryptionViewModel.EncryptedInputText!, key);
				encryptionViewModel.DecryptionResult = encryptionResult;
			}

			return View(encryptionViewModel);
		}
	}
}