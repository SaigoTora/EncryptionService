using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using EncryptionService.Configurations;
using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models.HomophonicEncryption;
using EncryptionService.Models;

namespace EncryptionService.Controllers
{
	public class HomophonicEncryptionController(
		IEncryptionService<HomophonicEncryptionResult, HomophonicEncryptionKey,
			Dictionary<char, int[]>> encryptionService,
		IOptions<EncryptionSettings> encryptionSettings)
		: Controller
	{
		static HomophonicEncryptionKey? _homophonicEncryptionKey = null;
		readonly IEncryptionService<HomophonicEncryptionResult, HomophonicEncryptionKey,
			Dictionary<char, int[]>> _encryptionService = encryptionService;
		readonly EncryptionSettings _encryptionSettings = encryptionSettings.Value;

		public IActionResult Index() => View();

		[HttpPost]
		public IActionResult Index(EncryptionViewModel<HomophonicEncryptionResult> encryptionViewModel,
			string actionType)
		{
			if (!ModelState.IsValid)
				return View(encryptionViewModel);

			if (_homophonicEncryptionKey == null)
			{
				Dictionary<char, int> frequency = _encryptionSettings.HomophonicEncryptionFrequency
					.ToDictionary(kvp => kvp.Key[0], kvp => kvp.Value);
				_homophonicEncryptionKey = HomophonicEncryptionKey.GenerateKey(frequency);
			}

			HomophonicEncryptionResult encryptionResult;

			if (actionType == "Encrypt")
			{
				encryptionResult = _encryptionService.Encrypt(encryptionViewModel.InputText!,
					_homophonicEncryptionKey);
				encryptionViewModel.EncryptionResult = encryptionResult;
			}
			else if (actionType == "Decrypt")
			{
				encryptionResult = _encryptionService.Decrypt(
					encryptionViewModel.EncryptedInputText!, _homophonicEncryptionKey);
				encryptionViewModel.DecryptionResult = encryptionResult;
			}

			return View(encryptionViewModel);
		}
	}
}