using System.Text;

using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models.TranspositionCiphers.VerticalTransposition;

namespace EncryptionService.Core.Services
{
	public class VerticalTranspositionEncryptionService
		: IEncryptionService<VerticalTranspositionEncryptionResult, VerticalTranspositionKey,
			string>
	{
		private const char FILL_CHAR = '.';

		public VerticalTranspositionEncryptionResult Encrypt(string text,
			VerticalTranspositionKey encryptionKey)
			=> ProcessEncryption(text, encryptionKey, true);
		public VerticalTranspositionEncryptionResult Decrypt(string encryptedText,
			VerticalTranspositionKey encryptionKey)
			=> ProcessEncryption(encryptedText, encryptionKey, false);

		private static VerticalTranspositionEncryptionResult ProcessEncryption(string text,
			VerticalTranspositionKey encryptionKey, bool isEncryption)
		{
			var sorted = encryptionKey.Key
				.Select((ch, index) => new { ch, index })
				.OrderBy(x => x.ch)
				.Select((x, newIndex) => new { x.index, newIndex })
				.ToDictionary(x => x.index, x => x.newIndex);

			char[,] matrix = new char[(int)Math.Ceiling((double)text.Length
				/ encryptionKey.Key.Length), encryptionKey.Key.Length];
			matrix = FillMatrix(matrix, text, sorted, isEncryption);

			StringBuilder builder = new();
			if (isEncryption)
				foreach (int j in sorted.OrderBy(x => x.Value).Select(x => x.Key))
					for (int i = 0; i < matrix.GetLength(0); i++)
						builder.Append(matrix[i, j]);
			else
				for (int i = 0; i < matrix.GetLength(0); i++)
					for (int j = 0; j < matrix.GetLength(1); j++)
						builder.Append(matrix[i, j]);

			int[] sortIndixes = [.. encryptionKey.Key.Select((_, i) => sorted[i])];
			return new VerticalTranspositionEncryptionResult(builder.ToString(), 
				matrix, sortIndixes);
		}
		private static char[,] FillMatrix(char[,] matrix, string text, Dictionary<int, int> sorted,
			bool isEncryption)
		{
			int k = 0;
			if (isEncryption)
			{
				for (int i = 0; i < matrix.GetLength(0); i++)
					for (int j = 0; j < matrix.GetLength(1); j++)
					{
						if (k < text.Length)
							matrix[i, j] = text[k++];
						else
							matrix[i, j] = FILL_CHAR;
					}
			}
			else
			{
				foreach (int j in sorted.OrderBy(x => x.Value).Select(x => x.Key))
					for (int i = 0; i < matrix.GetLength(0); i++)
					{
						if (k < text.Length)
							matrix[i, j] = text[k++];
						else
							matrix[i, j] = FILL_CHAR;
					}
			}

			return matrix;
		}
	}
}