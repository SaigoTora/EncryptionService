using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models;

namespace EncryptionService.Core.Services
{
	public class BlockTranspositionEncryptionService : IEncryptionService<BlockTranspositionKey, int[]>
	{
		private const char FILL_CHAR = '.';

		public EncryptionResult Encrypt(string text, BlockTranspositionKey encryptionKey)
			=> ProcessEncryption(text, encryptionKey, true);
		public EncryptionResult Decrypt(string encryptedText, BlockTranspositionKey encryptionKey)
			=> ProcessEncryption(encryptedText, encryptionKey, false);

		private static EncryptionResult ProcessEncryption(string text, BlockTranspositionKey encryptionKey,
			bool isEncryption)
		{
			while (text.Length % encryptionKey.Key.Length != 0)
				text += FILL_CHAR;

			char[] resultArr = new string(FILL_CHAR, text.Length).ToCharArray();
			int blockNumber = 0;

			for (int i = 0, keyIndex = 0; i < text.Length; i++, keyIndex++)
			{
				if (i == blockNumber * encryptionKey.Key.Length)
				{
					keyIndex = 0;
					blockNumber++;
				}

				int index = encryptionKey.Key[keyIndex] - 1
					+ (blockNumber - 1) * encryptionKey.Key.Length;
				if (isEncryption)
					resultArr[index] = text[i];
				else
					resultArr[i] = text[index];
			}

			return new(new string(resultArr));
		}
	}
}