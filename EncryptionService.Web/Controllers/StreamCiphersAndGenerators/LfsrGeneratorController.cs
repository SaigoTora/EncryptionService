using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models.StreamCiphersAndGenerators.IcgGenerator;
using EncryptionService.Web.Configurations;
using EncryptionService.Web.Models;

namespace EncryptionService.Web.Controllers.StreamCiphersAndGenerators
{
	public class LfsrGeneratorController(IRandomNumbersGenerator<IcgGeneratorParameters>
		randomNumberGenerator, IOptions<EncryptionSettings> encryptionSettings) : Controller
	{
		readonly IRandomNumbersGenerator<IcgGeneratorParameters> _randomNumberGenerator
			= randomNumberGenerator;
		readonly EncryptionSettings _encryptionSettings = encryptionSettings.Value;

		public IActionResult Index() => View();

		[HttpPost]
		public IActionResult Index(NumberGeneratorViewModel numberGeneratorViewModel)
		{
			if (!ModelState.IsValid)
				return View();

			IcgGeneratorParameters parameters = _encryptionSettings.IcgGeneratorParameters;

			List<int> generatedNumbers = _randomNumberGenerator.Generate(parameters,
				numberGeneratorViewModel.GammaLength, numberGeneratorViewModel.Seed);
			numberGeneratorViewModel.ResultNumbers = generatedNumbers;

			return View(numberGeneratorViewModel);
		}
	}
}