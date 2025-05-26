using Microsoft.AspNetCore.Mvc;

using EncryptionService.Core.Interfaces;
using EncryptionService.Web.Extensions;
using EncryptionService.Web.Models.HashingViewModels;

namespace EncryptionService.Web.Controllers.Hashing
{
    public class RsaSignatureController(IHashingService hashingService) : Controller
	{
		private readonly IHashingService _hashingService = hashingService;

		public IActionResult Index() => View();

		[HttpPost]
		public IActionResult Index(HashingViewModel hashingViewModel, string actionType)
		{
			if (!ModelState.IsValid)
				return View(hashingViewModel);

			if (actionType == "Hash")
			{
				if (!this.ValidateRequiredInput(hashingViewModel.TextToHash,
					nameof(hashingViewModel.TextToHash), "Text"))
					return View(hashingViewModel);

				hashingViewModel.GeneratedHash = _hashingService.ComputeHash(
					hashingViewModel.TextToHash!, hashingViewModel.HashingMethod);
			}
			else if (actionType == "Verify")
			{
				if (!this.ValidateRequiredInput(hashingViewModel.TextToVerify,
					nameof(hashingViewModel.TextToVerify), "Text"))
					return View(hashingViewModel);
				if (!this.ValidateRequiredInput(hashingViewModel.HashToVerify,
					nameof(hashingViewModel.HashToVerify), "Hash"))
					return View(hashingViewModel);

				hashingViewModel.VerificationResult = _hashingService.VerifyHash(
					hashingViewModel.TextToVerify!, hashingViewModel.HashToVerify!,
					hashingViewModel.HashingMethod);
			}

			return View(hashingViewModel);
		}
	}
}