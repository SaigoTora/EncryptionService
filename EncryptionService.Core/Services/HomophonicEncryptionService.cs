using System.Text;

using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models;
using EncryptionService.Core.Models.HomophonicEncryption;

namespace EncryptionService.Core.Services
{
	public class HomophonicEncryptionService
		: IEncryptionService<EncryptionResult, HomophonicEncryptionKey,
			Dictionary<char, int[]>>
	{
		public EncryptionResult Encrypt(string text, HomophonicEncryptionKey encryptionKey)
		{
			text = text.ToUpper();
			Random random = new();
			StringBuilder builder = new();
			Dictionary<char, List<int>> keyCopy = GenerateKeyCopy(encryptionKey);

			foreach (char ch in text)
			{
				if (keyCopy[ch].Count != 0)
				{
					List<int> numbers = keyCopy[ch];
					int randomIndex = random.Next(0, numbers.Count);
					builder.Append(numbers[randomIndex].ToString("D3"));
					numbers.RemoveAt(randomIndex);
				}
				else
				{
					int[] numbers = encryptionKey.Key[ch];
					int randomIndex = random.Next(0, numbers.Length);
					builder.Append(numbers[randomIndex].ToString("D3"));
				}
			}

			return new EncryptionResult(builder.ToString());
		}
		public EncryptionResult Decrypt(string encryptedText,
			HomophonicEncryptionKey encryptionKey)
		{
			StringBuilder builder = new();

			for (int i = 0; i < encryptedText.Length; i += 3)
			{
				int number = int.Parse(encryptedText.Substring(i, 3));
				foreach (var kvp in encryptionKey.Key)
				{
					if (kvp.Value.Contains(number))
					{
						builder.Append(kvp.Key);
						break;
					}
				}
			}

			return new EncryptionResult(builder.ToString());
		}

		private static Dictionary<char, List<int>> GenerateKeyCopy(
			HomophonicEncryptionKey encryptionKey)
		{
			Dictionary<char, List<int>> keyCopy = [];
			foreach (var kvp in encryptionKey.Key)
				keyCopy[kvp.Key] = [.. kvp.Value];

			return keyCopy;
		}
	}
}