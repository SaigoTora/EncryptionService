using System.Text;

using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models;
using EncryptionService.Core.Models.StreamCiphersAndGenerators.LfsrGenerator;

namespace EncryptionService.Core.Services.StreamCiphersAndGenerators
{
	public class LfsrGeneratorService : IEncryptionService<LfsrEncryptionResult,
		LfsrEncryptionKey, int[]>
	{
		public LfsrEncryptionResult Encrypt(string text, LfsrEncryptionKey encryptionKey)
			=> ProcessEncryption(text, encryptionKey, true);
		public LfsrEncryptionResult Decrypt(string encryptedText, LfsrEncryptionKey encryptionKey)
			=> ProcessEncryption(encryptedText, encryptionKey, false);

		private static LfsrEncryptionResult ProcessEncryption(string text,
			LfsrEncryptionKey encryptionKey, bool isEncryption)
		{
			byte[] inputBytes = GetInputBytes(text, encryptionKey.Format, isEncryption);
			byte[] outputBytes = new byte[inputBytes.Length];

			for (int i = 0; i < inputBytes.Length; i++)
			{
				bool[] bits = GetBits(encryptionKey);
				byte gammaByte = GetByteFromBits(bits);
				outputBytes[i] = (byte)(inputBytes[i] ^ gammaByte);
			}

			string resultText = GetResultText(outputBytes, isEncryption);
			string binary = string.Concat(outputBytes.Select(b
				=> Convert.ToString(b, 2).PadLeft(8, '0')));
			string hex = Convert.ToHexString(outputBytes);

			return new LfsrEncryptionResult(resultText, binary, hex);
		}
		private static byte[] GetInputBytes(string text, EncryptionFormat format, bool isEncryption)
		{
			if (isEncryption)
			{
				return format switch
				{
					EncryptionFormat.Text => Encoding.UTF8.GetBytes(text),
					EncryptionFormat.Binary => BinaryStringToBytes(text),
					EncryptionFormat.Hexadecimal => Convert.FromHexString(text),
					_ => throw new ArgumentOutOfRangeException(nameof(format),
						"Unsupported format.")
				};
			}
			else
			{
				return format switch
				{
					EncryptionFormat.Text => Convert.FromBase64String(text),
					EncryptionFormat.Binary => BinaryStringToBytes(text),
					EncryptionFormat.Hexadecimal => Convert.FromHexString(text),
					_ => throw new ArgumentOutOfRangeException(nameof(format),
						"Unsupported format.")
				};
			}
		}

		private static byte[] BinaryStringToBytes(string binary)
		{
			if (binary.Length % 8 != 0)
				throw new ArgumentException("Binary string length must be multiple of 8.");

			int byteCount = binary.Length / 8;
			byte[] result = new byte[byteCount];

			for (int i = 0; i < byteCount; i++)
			{
				string byteStr = binary.Substring(i * 8, 8);
				result[i] = Convert.ToByte(byteStr, 2);
			}

			return result;
		}
		private static string GetResultText(byte[] bytes, bool isEncryption)
		{
			if (isEncryption)
				return Convert.ToBase64String(bytes);

			return Encoding.UTF8.GetString(bytes);
		}

		private static bool[] GetBits(LfsrEncryptionKey key)
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