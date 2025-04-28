using EncryptionService.Core.Interfaces;

namespace EncryptionService.Core.Models.SubstitutionCiphers.HomophonicEncryption
{
	public class HomophonicEncryptionKey : IEncryptionKey<Dictionary<char, int[]>>
	{
		public required Dictionary<char, int[]> Key { get; init; }

		public static HomophonicEncryptionKey GenerateKey(Dictionary<char, int> letterFrequency)
		{
			HashSet<int> uniqueNumbers = GetRandomNumbers(1000);

			Dictionary<char, int[]> encryptionKey = [];
			int k = 0;
			foreach (var frequencyKVP in letterFrequency)
			{
				int[] arr = new int[frequencyKVP.Value];
				for (int i = 0; i < arr.Length; i++)
					arr[i] = uniqueNumbers.ElementAt(k++);

				encryptionKey.Add(frequencyKVP.Key, arr);
			}

			return new HomophonicEncryptionKey() { Key = encryptionKey };
		}
		private static HashSet<int> GetRandomNumbers(int count)
		{
			Random random = new Random();
			var numbers = Enumerable.Range(0, count).ToList();
			numbers = [.. numbers.OrderBy(x => random.Next())];
			HashSet<int> uniqueNumbers = [.. numbers];

			return uniqueNumbers;
		}
	}
}