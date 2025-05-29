using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models.AsymmetricEncryption.RsaEncryption;
using EncryptionService.Web.Configurations;
using EncryptionService.Web.Extensions;
using EncryptionService.Web.Models.HashingViewModels;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

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
		public IActionResult Sign(HashingViewModel model)
		{
			if (!ModelState.IsValid)
				return View("Index", model);

			RsaEncryptionKey key = _encryptionSettings.RsaEncryptionKey;
			model.GeneratedHash = _signatureService.CreateSignature(model.TextToHash!, key);

			return View("Index", model);
		}

		[HttpPost]
		public IActionResult Verify(HashingViewModel model)
		{
			if (!ModelState.IsValid)
				return View("Index", model);

			RsaEncryptionKey key = _encryptionSettings.RsaEncryptionKey;
			model.VerificationResult = _signatureService.VerifySignature(model.TextToVerify!,
				model.HashToVerify!, key);

			return View("Index", model);
		}
	}
}