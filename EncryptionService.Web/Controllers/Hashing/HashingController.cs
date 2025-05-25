using Microsoft.AspNetCore.Mvc;

using EncryptionService.Web.Extensions;
using EncryptionService.Web.Models.HashingViewModels;

namespace EncryptionService.Web.Controllers.Hashing
{
	public class HashingController()
		: Controller
	{
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

				hashingViewModel.GeneratedHash = "Generated Hash";
			}
			else if (actionType == "Verify")
			{
				if (!this.ValidateRequiredInput(hashingViewModel.TextToVerify,
					nameof(hashingViewModel.TextToVerify), "Text"))
					return View(hashingViewModel);
				if (!this.ValidateRequiredInput(hashingViewModel.HashToVerify,
					nameof(hashingViewModel.HashToVerify), "Hash"))
					return View(hashingViewModel);

				hashingViewModel.VerificationResult = true;
			}

			return View(hashingViewModel);
		}
	}
}