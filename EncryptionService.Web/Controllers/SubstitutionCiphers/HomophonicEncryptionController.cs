using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models.SubstitutionCiphers.HomophonicEncryption;
using EncryptionService.Web.Configurations;
using EncryptionService.Web.Models.EncryptionViewModels;

namespace EncryptionService.Web.Controllers.SubstitutionCiphers
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
		public IActionResult Encrypt(EncryptionViewModel<HomophonicEncryptionResult> model)
		{
			if (!ModelState.IsValid)
				return View("Index", model);

			if (_homophonicEncryptionKey == null)
			{
				Dictionary<char, int> frequency = _encryptionSettings.HomophonicEncryptionFrequency
					.ToDictionary(kvp => kvp.Key[0], kvp => kvp.Value);
				_homophonicEncryptionKey = HomophonicEncryptionKey.GetUniqueInstance(frequency);
			}

			model.EncryptionResult = _encryptionService.Encrypt(model.InputText!,
				_homophonicEncryptionKey!);
			return View("Index", model);
		}

		[HttpPost]
		public IActionResult Decrypt(EncryptionViewModel<HomophonicEncryptionResult> model)
		{
			if (!ModelState.IsValid)
				return View("Index", model);

			if (_homophonicEncryptionKey == null)
			{
				Dictionary<char, int> frequency = _encryptionSettings.HomophonicEncryptionFrequency
					.ToDictionary(kvp => kvp.Key[0], kvp => kvp.Value);
				_homophonicEncryptionKey = HomophonicEncryptionKey.GetUniqueInstance(frequency);
			}

			if (!IsEncryptedInputTextValid(model))
				return View("Index", model);

			model.DecryptionResult = _encryptionService.Decrypt(model.EncryptedInputText,
				_homophonicEncryptionKey!);
			return View("Index", model);
		}

		private bool IsEncryptedInputTextValid(
			EncryptionViewModel<HomophonicEncryptionResult> model)
		{
			if (model.EncryptedInputText!.Length % 3 != 0)
			{
				ModelState.AddModelError("EncryptedInputText",
					"The length of the encrypted text must be a multiple of 3.");
				return false;
			}
			else if (model.EncryptedInputText.Any(ch => !char.IsDigit(ch)))
			{
				ModelState.AddModelError("EncryptedInputText",
					"The encrypted text must contain only digits.");
				return false;
			}

			return true;
		}
	}
}