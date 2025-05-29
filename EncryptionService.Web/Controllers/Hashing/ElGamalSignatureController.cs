using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models.AsymmetricEncryption.ElGamalEncryption;
using EncryptionService.Core.Services.Hashing;
using EncryptionService.Web.Configurations;
using EncryptionService.Web.Extensions;
using EncryptionService.Web.Models.HashingViewModels;
using System.Reflection;

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
		public IActionResult Sign(HashingViewModel model)
		{
			if (!ModelState.IsValid)
				return View("Index", model);

			ElGamalEncryptionKey key = _encryptionSettings.ElGamalEncryptionKey;
			model.GeneratedHash = _signatureService.CreateSignature(model.TextToHash!, key);

			return View("Index", model);
		}

		[HttpPost]
		public IActionResult Verify(HashingViewModel model)
		{
			if (!ModelState.IsValid)
				return View("Index", model);

			ElGamalEncryptionKey key = _encryptionSettings.ElGamalEncryptionKey;
			if (!IsSignatureValid(model))
				return View("Index", model);

			model.VerificationResult = _signatureService.VerifySignature(
				model.TextToVerify!, model.HashToVerify!, key);

			return View("Index", model);
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