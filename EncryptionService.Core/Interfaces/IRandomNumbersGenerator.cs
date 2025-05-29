namespace EncryptionService.Core.Interfaces
{
	public interface IRandomNumbersGenerator<TParam> where TParam : IGeneratorParameters
	{
		List<int> Generate(TParam parameters, int gammaLength, int seed);
	}
}