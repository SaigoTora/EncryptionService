using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models.AsymmetricEncryption.RsaEncryption;
using EncryptionService.Web.Configurations;
using EncryptionService.Web.Extensions;
using EncryptionService.Web.Models.HashingViewModels;

namespace EncryptionService.Web.Controllers.Hashing
{
	public class RsaSignatureController(ISignatureService<RsaEncryptionKey,
		RsaEncryptionKeyData> signatureService, IOptions<EncryptionSettings> encryptionSettings)
		: Controller
	{
		private readonly ISignatureService<RsaEncryptionKey, RsaEncryptionKeyData>
			_signatureService = signatureService;
		private readonly EncryptionSettings _encryptionSettings = encryptionSettings.Value;

		public IActionResult Index() => View();

		[HttpPost]
		public IActionResult Index(HashingViewModel hashingViewModel, string actionType)
		{
			if (!ModelState.IsValid)
				return View(hashingViewModel);


			RsaEncryptionKey key = _encryptionSettings.RsaEncryptionKey;
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

				hashingViewModel.VerificationResult = _signatureService.VerifySignature(
					hashingViewModel.TextToVerify!, hashingViewModel.HashToVerify!, key);
			}

			return View(hashingViewModel);
		}
	}
}