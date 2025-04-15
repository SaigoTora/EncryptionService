namespace EncryptionService.Core.Interfaces
{
	public interface IRandomNumbersGenerator<TParam> where TParam : IGeneratorParameters
	{
		public List<int> Generate(TParam parameters, int gammaLength, int seed);
	}
}