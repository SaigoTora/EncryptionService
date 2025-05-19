using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models.SubstitutionCiphers.HomophonicEncryption;
using EncryptionService.Web.Configurations;
using EncryptionService.Web.Models.EncryptionViewModels;
using EncryptionService.Web.Extensions;

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
		public IActionResult Index(
			EncryptionViewModel<HomophonicEncryptionResult> encryptionViewModel,
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

			if (actionType == "Encrypt")
				return ProcessEncrypt(encryptionViewModel);
			else if (actionType == "Decrypt")
				return ProcessDecrypt(encryptionViewModel);

			return View(encryptionViewModel);
		}
		private ViewResult ProcessEncrypt(EncryptionViewModel<HomophonicEncryptionResult> model)
		{
			if (!this.ValidateRequiredInput(model.InputText,
				nameof(model.InputText), "Text"))
				return View(model);

			model.EncryptionResult = _encryptionService.Encrypt(model.InputText!,
				_homophonicEncryptionKey!);

			return View(model);
		}
		private ViewResult ProcessDecrypt(EncryptionViewModel<HomophonicEncryptionResult> model)
		{
			if (!this.ValidateRequiredInput(model.EncryptedInputText,
				nameof(model.EncryptedInputText), "Encrypted text"))
				return View(model);

			if (model.EncryptedInputText!.Length % 3 != 0)
			{
				ModelState.AddModelError("EncryptedInputText",
					"The length of the encrypted text must be a multiple of 3.");
				return View(model);
			}
			else if (model.EncryptedInputText.Any(ch => !char.IsDigit(ch)))
			{
				ModelState.AddModelError("EncryptedInputText",
					"The encrypted text must contain only digits.");
				return View(model);
			}

			model.DecryptionResult = _encryptionService.Decrypt(
				model.EncryptedInputText, _homophonicEncryptionKey!);

			return View(model);
		}
	}
}