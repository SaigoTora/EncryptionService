using System.Text;

using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models.Hashing;

namespace EncryptionService.Core.Services.Hashing
{
	public class HashingService : IHashingService
	{
		private const int CHECKSUM_BLOCK_SIZE = 3;

		public string ComputeHash(string text, HashingMethod method)
		{
			return method switch
			{
				HashingMethod.Checksum => ComputeChecksumHash(text),
				HashingMethod.MiddleSquares => "",
				HashingMethod.ModuloDivision => "",
				HashingMethod.BaseConversion => "",
				HashingMethod.Folding => "",
				_ => string.Empty,
			};
		}
		public bool VerifyHash(string text, string hash, HashingMethod method)
			=> hash == ComputeHash(text, method);

		private static string ComputeChecksumHash(string text)
		{
			int[] checksums = new int[CHECKSUM_BLOCK_SIZE];

			for (int i = 0; i < text.Length; i++)
			{
				int blockIndex = i % CHECKSUM_BLOCK_SIZE;
				checksums[blockIndex] ^= text[i];
			}

			StringBuilder builder = new();
			for (int i = 0; i < checksums.Length; i++)
				builder.Append(checksums[i].ToString("X2"));

			return builder.ToString();
		}
	}
}