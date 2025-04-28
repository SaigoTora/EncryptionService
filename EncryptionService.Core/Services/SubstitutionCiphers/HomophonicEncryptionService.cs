using System.Text;

using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models.SubstitutionCiphers.HomophonicEncryption;

namespace EncryptionService.Core.Services.SubstitutionCiphers
{
	public class HomophonicEncryptionService
		: IEncryptionService<HomophonicEncryptionResult, HomophonicEncryptionKey,
			Dictionary<char, int[]>>
	{
		public HomophonicEncryptionResult Encrypt(string text,
			HomophonicEncryptionKey encryptionKey)
		{
			text = text.ToUpper();
			Random random = new();
			StringBuilder builder = new();
			Dictionary<char, List<int>> keyCopy = GenerateKeyCopy(encryptionKey);

			foreach (char ch in text)
			{
				if (keyCopy.TryGetValue(ch, out var listNumbers))
				{
					if (listNumbers.Count != 0)
					{
						int randomIndex = random.Next(0, listNumbers.Count);
						builder.Append(listNumbers[randomIndex].ToString("D3"));
						listNumbers.RemoveAt(randomIndex);
					}
					else
					{
						int[] arrNumbers = encryptionKey.Key[ch];
						int randomIndex = random.Next(0, arrNumbers.Length);
						builder.Append(arrNumbers[randomIndex].ToString("D3"));
					}
				}
			}

			return new HomophonicEncryptionResult(builder.ToString(), encryptionKey.Key);
		}
		public HomophonicEncryptionResult Decrypt(string encryptedText,
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

			return new HomophonicEncryptionResult(builder.ToString(), encryptionKey.Key);
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