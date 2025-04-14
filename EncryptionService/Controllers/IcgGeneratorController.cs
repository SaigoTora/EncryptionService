using Microsoft.AspNetCore.Mvc;

using EncryptionService.Models;

namespace EncryptionService.Controllers
{
	public class IcgGeneratorController : Controller
	{
		public IActionResult Index() => View();

		[HttpPost]
		public IActionResult Index(NumberGeneratorViewModel numberGeneratorViewModel)
		{
			if (!ModelState.IsValid)
				return View();

			int generatedNumber = 1;
			numberGeneratorViewModel.ResultNumber = generatedNumber;

			return View(numberGeneratorViewModel);
		}
	}
}