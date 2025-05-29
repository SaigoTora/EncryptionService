using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models.StreamCiphersAndGenerators.IcgGenerator;
using EncryptionService.Web.Configurations;
using EncryptionService.Web.Models.EncryptionViewModels;

namespace EncryptionService.Web.Controllers.StreamCiphersAndGenerators
{
	public class IcgGeneratorController(IRandomNumbersGenerator<IcgGeneratorParameters>
		randomNumberGenerator, IOptions<EncryptionSettings> encryptionSettings) : Controller
	{
		readonly IRandomNumbersGenerator<IcgGeneratorParameters> _randomNumberGenerator
			= randomNumberGenerator;
		readonly EncryptionSettings _encryptionSettings = encryptionSettings.Value;

		public IActionResult Index() => View();

		[HttpPost]
		public IActionResult Generate(NumberGeneratorViewModel model)
		{
			if (!ModelState.IsValid)
				return View("Index", model);

			IcgGeneratorParameters parameters = _encryptionSettings.IcgGeneratorParameters;

			model.ResultNumbers = _randomNumberGenerator.Generate(parameters, model.GammaLength!.Value,
				model.Seed!.Value);

			return View("Index", model);
		}
	}
}