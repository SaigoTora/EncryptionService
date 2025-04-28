using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models.StreamCiphersAndGenerators.IcgGenerator;

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

			return (parameters.A * ModInverse(seed, parameters.M) + parameters.B) % parameters.M;
		}
		private static int ModInverse(int a, int m)
		{
			int b0 = m, t, q;
			int x0 = 0, x1 = 1;
			if (m == 1) return 1;

			while (a > 1)
			{
				q = a / m;
				t = m;
				m = a % m;
				a = t;
				t = x0;
				x0 = x1 - q * x0;
				x1 = t;
			}

			if (x1 < 0)
				x1 += b0;

			return x1;
		}
	}
}