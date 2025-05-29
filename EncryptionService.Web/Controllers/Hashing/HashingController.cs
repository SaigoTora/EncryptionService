using Microsoft.AspNetCore.Mvc;

using EncryptionService.Web.Models.HashingViewModels;
using EncryptionService.Core.Interfaces;

namespace EncryptionService.Web.Controllers.Hashing
{
	public class HashingController(IHashingService hashingService) : Controller
	{
		private readonly IHashingService _hashingService = hashingService;

		public IActionResult Index() => View();

		[HttpPost]
		public IActionResult CreateHash(HashingViewModel model)
		{
			if (!ModelState.IsValid)
				return View("Index", model);

			model.GeneratedHash = _hashingService.ComputeHash(model.TextToHash!,
				model.CreatingHashingMethod);

			return View("Index", model);
		}

		[HttpPost]
		public IActionResult Verify(HashingViewModel model)
		{
			if (!ModelState.IsValid)
				return View("Index", model);

			model.VerificationResult = _hashingService.VerifyHash(model.TextToVerify!,
				model.HashToVerify!, model.VerifyingHashingMethod);

			return View("Index", model);
		}
	}
}