using EncryptionService.Core.Interfaces;

namespace EncryptionService.Core.Models.SubstitutionCiphers.HomophonicEncryption
{
	public class HomophonicEncryptionKey : IEncryptionKey<Dictionary<char, int[]>>
	{
		public Dictionary<char, int[]> Key { get; init; } = [];
		private static HomophonicEncryptionKey? _uniqueInstance;

		private HomophonicEncryptionKey(Dictionary<char, int> letterFrequency)
		{
			GenerateKey(letterFrequency);
		}

		public static HomophonicEncryptionKey GetUniqueInstance(
			Dictionary<char, int> letterFrequency)
		{
			if (_uniqueInstance == null)
				_uniqueInstance = new HomophonicEncryptionKey(letterFrequency);

			return _uniqueInstance;
		}
		public void GenerateKey(Dictionary<char, int> letterFrequency)
		{
			HashSet<int> uniqueNumbers = GetRandomNumbers(1000);

			int k = 0;
			foreach (var frequencyKVP in letterFrequency)
			{
				int[] arr = new int[frequencyKVP.Value];
				for (int i = 0; i < arr.Length; i++)
					arr[i] = uniqueNumbers.ElementAt(k++);

				Key.Add(frequencyKVP.Key, arr);
			}
		}
		private static HashSet<int> GetRandomNumbers(int count)
		{
			Random random = new();
			var numbers = Enumerable.Range(0, count).ToList();
			numbers = [.. numbers.OrderBy(x => random.Next())];
			HashSet<int> uniqueNumbers = [.. numbers];

			return uniqueNumbers;
		}
	}
}