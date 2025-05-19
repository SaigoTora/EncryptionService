using System.Text;

using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models;
using EncryptionService.Core.Models.AsymmetricEncryption.RsaEncryption;

namespace EncryptionService.Core.Services.AsymmetricEncryption
{
	public class RsaEncryptionService : IEncryptionService<EncryptionResult,
		RsaEncryptionKey, RsaEncryptionKeyData>
	{
		private const string SEPARATOR = "-";

		private record EncryptionValues(int N, int EulerPhi, int E, int D);
		private int? _e;
		private readonly Random _random = new();

		public EncryptionResult Encrypt(string text, RsaEncryptionKey encryptionKey)
			=> ProcessEncryption(text, encryptionKey, true);
		public EncryptionResult Decrypt(string encryptedText, RsaEncryptionKey encryptionKey)
			=> ProcessEncryption(encryptedText, encryptionKey, false);
		private EncryptionResult ProcessEncryption(string text,
			RsaEncryptionKey encryptionKey, bool isEncryption)
		{
			EncryptionValues values = GenerateEncryptionValues(encryptionKey.Key, !isEncryption);

			StringBuilder builder = new();
			if (isEncryption)
			{
				List<int> encrypted = [];
				foreach (char ch in text)
					encrypted.Add(ModPow(ch, values.E, values.N));

				builder.Append(string.Join(SEPARATOR, encrypted));
			}
			else
			{
				List<char> decrypted = [];
				foreach (string token in text.Split(SEPARATOR))
				{
					int num = int.Parse(token);
					decrypted.Add((char)ModPow(num, values.D, values.N));
				}
				builder.Append(new string([.. decrypted]));

			}

			return new(builder.ToString());
		}
		private EncryptionValues GenerateEncryptionValues(RsaEncryptionKeyData keyData,
			bool calculateD)
		{
			int n = keyData.P * keyData.Q;
			int eulerPhi = (keyData.P - 1) * (keyData.Q - 1);
			GenerateE(eulerPhi);
			if (!_e.HasValue)
				throw new InvalidOperationException("Encryption key is not set.");

			int d = 0;
			if (calculateD)
				d = ModInverse(_e.Value, eulerPhi);

			return new EncryptionValues(n, eulerPhi, _e.Value, d);
		}

		private void GenerateE(int eulerPhi)
		{
			if (_e.HasValue)
				return;

			List<int> numbers = [];
			for (int i = 2; i < eulerPhi; i++)
				if (i % 2 != 0 && GCD(i, eulerPhi) == 1)
					numbers.Add(i);

			_e = numbers[_random.Next(numbers.Count)];
		}
		private static int GCD(int a, int b)
		{
			while (b != 0)
			{
				int temp = b;
				b = a % b;
				a = temp;
			}
			return a;
		}

		private static int ModInverse(int a, int mod)
		{
			int t = 0, newT = 1;
			int r = mod, newR = a;

			while (newR != 0)
			{
				int quotient = r / newR;

				(t, newT) = (newT, t - quotient * newT);
				(r, newR) = (newR, r - quotient * newR);
			}

			if (r > 1)
				throw new ArgumentException("The number does not have an inverse modulus.");

			if (t < 0)
				t += mod;

			return t;
		}
		private static int ModPow(int baseValue, int exponent, int mod)
		{
			long result = 1;
			long baseMod = baseValue % mod;

			while (exponent > 0)
			{
				if ((exponent & 1) == 1)
					result = (result * baseMod) % mod;

				baseMod = (baseMod * baseMod) % mod;
				exponent >>= 1;
			}

			return (int)result;
		}
	}
}