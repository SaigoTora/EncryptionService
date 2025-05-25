using System.Text;

using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models.Hashing;

namespace EncryptionService.Core.Services.Hashing
{
	public class HashingService : IHashingService
	{
		private const int BLOCK_SIZE = 3;
		private const int MODULO_HASH_DIVISOR = 13;
		private const int P_BASE = 2;
		private const int Q_BASE = 4;
		private const int ADDRESS_SIZE = 4;
		private const ulong MAX_ADDRESS_VALUE = (1UL << (ADDRESS_SIZE * 8)) - 1;

		public string ComputeHash(string text, HashingMethod method)
		{
			return method switch
			{
				HashingMethod.Checksum => ComputeChecksumHash(text),
				HashingMethod.MiddleSquares => ComputeMiddleSquaresHash(text),
				HashingMethod.ModuloDivision => ComputeModuloDivisionHash(text),
				HashingMethod.BaseConversion => ComputeBaseConversionHash(text),
				HashingMethod.Folding => ComputeFoldingHash(text),
				_ => string.Empty,
			};
		}
		public bool VerifyHash(string text, string hash, HashingMethod method)
			=> hash == ComputeHash(text, method);

		public static string ConvertNumbersToString(int[] numbers)
		{
			StringBuilder builder = new();
			for (int i = 0; i < numbers.Length; i++)
				builder.Append(numbers[i].ToString("X2"));

			return builder.ToString();
		}
		public static string ConvertNumbersToString(ulong[] numbers)
		{
			StringBuilder builder = new();
			for (int i = 0; i < numbers.Length; i++)
				builder.Append(numbers[i].ToString("X2"));

			return builder.ToString();
		}

		private static string ComputeChecksumHash(string text)
		{
			int[] numbers = new int[BLOCK_SIZE];

			for (int i = 0; i < text.Length; i++)
			{
				int blockIndex = i % BLOCK_SIZE;
				numbers[blockIndex] ^= text[i];
			}

			return ConvertNumbersToString(numbers);
		}

		private static string ComputeMiddleSquaresHash(string text)
		{
			int[] numbers = new int[BLOCK_SIZE];

			for (int i = 0; i < text.Length; i++)
			{
				int square = text[i] * text[i];
				int blockIndex = i % BLOCK_SIZE;

				numbers[blockIndex] |= GetMiddleDigits(square);
			}

			return ConvertNumbersToString(numbers);
		}
		private static int GetMiddleDigits(int number)
		{
			string numStr = number.ToString();
			int len = numStr.Length;

			if (len <= 2)
				return number;

			int mid = len / 2;

			return Convert.ToInt32(numStr[mid] + numStr[mid - 1].ToString());
		}

		private static string ComputeModuloDivisionHash(string text)
		{
			int[] numbers = new int[BLOCK_SIZE];

			for (int i = 0; i < text.Length; i++)
			{
				int blockIndex = i % BLOCK_SIZE;
				numbers[blockIndex] += text[i] % MODULO_HASH_DIVISOR;
			}

			return ConvertNumbersToString(numbers);
		}

		private static string ComputeBaseConversionHash(string text)
		{
			int[] numbers = new int[BLOCK_SIZE];

			for (int i = 0; i < text.Length; i++)
			{
				int charCode = text[i];

				string pBaseStr = Convert.ToString(charCode, P_BASE);
				int qValue = 0;
				for (int j = 0; j < pBaseStr.Length; j++)
					if (pBaseStr[j] == '1')
						qValue += (int)Math.Pow(Q_BASE, pBaseStr.Length - j - 1);

				int blockIndex = i % BLOCK_SIZE;
				numbers[blockIndex] += qValue % charCode;
			}

			return ConvertNumbersToString(numbers);
		}

		private static string ComputeFoldingHash(string text)
		{
			ulong[] numbers = new ulong[BLOCK_SIZE];

			for (int i = 0; i < text.Length; i++)
			{
				int blockIndex = i % BLOCK_SIZE;
				numbers[blockIndex] = (numbers[blockIndex] + (ulong)text[i]) & MAX_ADDRESS_VALUE;
			}

			return ConvertNumbersToString(numbers);
		}
	}
}