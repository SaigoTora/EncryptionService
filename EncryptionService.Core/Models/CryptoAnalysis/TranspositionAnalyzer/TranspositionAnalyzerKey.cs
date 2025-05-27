using EncryptionService.Core.Interfaces;

namespace EncryptionService.Core.Models.CryptoAnalysis.TranspositionAnalyzer
{
	public class TranspositionAnalyzerKey : IEncryptionKey<HashSet<int>>
	{
		public HashSet<int> Key { get; init; } = [];
		private readonly Random _random = new();

		public TranspositionAnalyzerKey(int KeyLength)
		{
			GenerateKey(KeyLength);
		}

		private void GenerateKey(int KeyLength)
		{
			List<int> numbers = new(KeyLength);
			for (int i = 0; i < KeyLength; i++)
				numbers.Add(i);

			while (numbers.Count > 0)
			{
				int randomNumber = numbers[_random.Next(numbers.Count)];
				Key.Add(randomNumber);
				numbers.Remove(randomNumber);
			}
		}
	}
}