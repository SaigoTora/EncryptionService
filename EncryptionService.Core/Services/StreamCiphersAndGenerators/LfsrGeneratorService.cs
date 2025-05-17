using System.Text;

using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models.StreamCiphersAndGenerators.LfsrGenerator;

namespace EncryptionService.Core.Services.StreamCiphersAndGenerators
{
	public class LfsrGeneratorService : IEncryptionService<LfsrEncryptionResult,
		LfsrGeneratorKey, int[]>
	{
		public LfsrEncryptionResult Encrypt(string text, LfsrGeneratorKey encryptionKey)
		{
			byte[] inputBytes = Encoding.UTF8.GetBytes(text);
			byte[] encryptedBytes = new byte[inputBytes.Length];

			for (int i = 0; i < inputBytes.Length; i++)
			{
				bool[] bits = GetBits(encryptionKey);
				byte gammaByte = GetByteFromBits(bits);
				encryptedBytes[i] = (byte)(inputBytes[i] ^ gammaByte);
			}

			string encryptedBase64 = Convert.ToBase64String(encryptedBytes);
			string binary = string.Concat(encryptedBytes.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')));
			string hex = BitConverter.ToString(encryptedBytes).Replace("-", "");

			return new LfsrEncryptionResult(encryptedBase64, binary, hex);
		}
		public LfsrEncryptionResult Decrypt(string encryptedBase64, LfsrGeneratorKey encryptionKey)
		{
			byte[] encryptedBytes = Convert.FromBase64String(encryptedBase64);
			byte[] decryptedBytes = new byte[encryptedBytes.Length];

			for (int i = 0; i < encryptedBytes.Length; i++)
			{
				bool[] bits = GetBits(encryptionKey);
				byte gammaByte = GetByteFromBits(bits);
				decryptedBytes[i] = (byte)(encryptedBytes[i] ^ gammaByte);
			}

			string decryptedText = Encoding.UTF8.GetString(decryptedBytes);
			string binary = string.Concat(decryptedBytes.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')));
			string hex = BitConverter.ToString(decryptedBytes).Replace("-", "");

			return new LfsrEncryptionResult(decryptedText, binary, hex);
		}

		private static bool[] GetBits(LfsrGeneratorKey key)
		{
			int[] polynomial = key.Key;
			int stateLength = key.InitialState.Length;
			bool[] result = new bool[stateLength];
			result[0] = key.InitialState[stateLength - 1];

			for (int i = 1; i < stateLength; i++)
			{
				bool InputBit = key.InitialState[stateLength - 1 - polynomial[1]];
				for (int j = 2; j < polynomial.Length; j++)
					InputBit ^= key.InitialState[stateLength - 1 - polynomial[j]];

				for (int j = stateLength - 1; j > 0; j--)// Shift
					key.InitialState[j] = key.InitialState[j - 1];
				key.InitialState[0] = InputBit;
				result[i] = InputBit;
			}

			return result;
		}
		private static byte GetByteFromBits(bool[] bits)
		{
			byte result = 0;
			int index = 8 - bits.Length;

			foreach (bool b in bits)
			{
				if (b)
					result |= (byte)(1 << (7 - index));

				index++;
			}

			return result;
		}
	}
}