using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models.AsymmetricEncryption.ElGamalEncryption;
using EncryptionService.Core.Services.Hashing;
using EncryptionService.Web.Configurations;
using EncryptionService.Web.Extensions;
using EncryptionService.Web.Models.HashingViewModels;

namespace EncryptionService.Web.Controllers.Hashing
{
	public class ElGamalSignatureController(ISignatureService<ElGamalEncryptionKey,
		ElGamalEncryptionKeyData> signatureService, IOptions<EncryptionSettings> encryptionSettings)
		: Controller
	{
		private readonly ISignatureService<ElGamalEncryptionKey, ElGamalEncryptionKeyData>
			_signatureService = signatureService;
		private readonly EncryptionSettings _encryptionSettings = encryptionSettings.Value;

		public IActionResult Index() => View();

		[HttpPost]
		public IActionResult Index(HashingViewModel hashingViewModel, string actionType)
		{
			if (!ModelState.IsValid)
				return View(hashingViewModel);


			ElGamalEncryptionKey key = _encryptionSettings.ElGamalEncryptionKey;
			if (actionType == "Sign")
			{
				if (!this.ValidateRequiredInput(hashingViewModel.TextToHash,
					nameof(hashingViewModel.TextToHash), "Text"))
					return View(hashingViewModel);

				hashingViewModel.GeneratedHash = _signatureService.CreateSignature(
					hashingViewModel.TextToHash!, key);
			}
			else if (actionType == "Verify")
			{
				if (!this.ValidateRequiredInput(hashingViewModel.TextToVerify,
					nameof(hashingViewModel.TextToVerify), "Text"))
					return View(hashingViewModel);
				if (!this.ValidateRequiredInput(hashingViewModel.HashToVerify,
					nameof(hashingViewModel.HashToVerify), "Electronic digital signature"))
					return View(hashingViewModel);
				if (!IsSignatureValid(hashingViewModel))
					return View(hashingViewModel);

				hashingViewModel.VerificationResult = _signatureService.VerifySignature(
					hashingViewModel.TextToVerify!, hashingViewModel.HashToVerify!, key);
			}

			return View(hashingViewModel);
		}

		private bool IsSignatureValid(HashingViewModel model)
		{
			foreach (char ch in model.HashToVerify ?? string.Empty)
				if (!char.IsDigit(ch) && ElGamalSignatureService.SEPARATOR != ch)
				{
					ModelState.AddModelError(nameof(model.HashToVerify),
						$"The electronic digital signature can only contain digits and " +
						$"the separator '{ElGamalSignatureService.SEPARATOR}' symbol.");
					return false;
				}

			return true;
		}
	}
}