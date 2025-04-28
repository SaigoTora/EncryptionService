using Microsoft.AspNetCore.Mvc;

using EncryptionService.Models;
using EncryptionService.Configurations;
using Microsoft.Extensions.Options;
using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models.StreamCiphersAndGenerators.IcgGenerator;

namespace EncryptionService.Controllers.StreamCiphersAndGenerators
{
	public class IcgGeneratorController(IRandomNumbersGenerator<IcgGeneratorParameters>
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