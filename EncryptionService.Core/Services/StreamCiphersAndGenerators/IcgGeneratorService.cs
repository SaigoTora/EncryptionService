using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models.StreamCiphersAndGenerators.IcgGenerator;
using EncryptionService.Core.Utils;

namespace EncryptionService.Core.Services.StreamCiphersAndGenerators
{
	public class IcgGeneratorService : IRandomNumbersGenerator<IcgGeneratorParameters>
	{
		public List<int> Generate(IcgGeneratorParameters parameters, int gammaLength, int seed)
		{
			var gamma = new List<int>() { seed };

			for (int i = 0; i < gammaLength - 1; i++)
			{
				seed = GenerateNumber(parameters, seed);
				gamma.Add(seed);
			}

			return gamma;
		}

		private static int GenerateNumber(IcgGeneratorParameters parameters, int seed)
		{
			if (seed == 0)
				return parameters.B;

			int modInverseSeed;
			try
			{
				modInverseSeed = MathUtils.ModInverse(seed, parameters.M);
			}
			catch (ArgumentException)
			{
				modInverseSeed = 1;
			}

			return (parameters.A * modInverseSeed + parameters.B)
				% parameters.M;
		}
	}
}