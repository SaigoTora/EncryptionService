using System.Numerics;

using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models.AsymmetricEncryption.RsaEncryption;
using EncryptionService.Core.Utils;

namespace EncryptionService.Core.Services.AsymmetricEncryption
{
	public class RsaEncryptionService : IEncryptionService<RsaEncryptionResult,
		RsaEncryptionKey, RsaEncryptionKeyData>
	{
		public const string SEPARATOR = " ";

		private record EncryptionValues(int N, int EulerPhi, int E, int D);
		private int? _e;
		private readonly Random _random = new();

		public RsaEncryptionResult Encrypt(string text, RsaEncryptionKey encryptionKey)
			=> ProcessEncryption(text, encryptionKey, true);
		public RsaEncryptionResult Decrypt(string encryptedText, RsaEncryptionKey encryptionKey)
			=> ProcessEncryption(encryptedText, encryptionKey, false);
		private RsaEncryptionResult ProcessEncryption(string text,
			RsaEncryptionKey encryptionKey, bool isEncryption)
		{
			EncryptionValues values = GenerateEncryptionValues(encryptionKey.Key, !isEncryption);

			string resultText;
			if (isEncryption)
			{
				List<int> encrypted = [];
				foreach (char ch in text)
					encrypted.Add((int)BigInteger.ModPow(ch, values.E, values.N));

				resultText = string.Join(SEPARATOR, encrypted);
			}
			else
			{
				List<char> decrypted = [];
				foreach (string token in text.Split(SEPARATOR))
				{
					int num = int.Parse(token);
					decrypted.Add((char)BigInteger.ModPow(num, values.D, values.N));
				}
				resultText = new string([.. decrypted]);
			}

			if (isEncryption)
				return new RsaEncryptionResult(resultText, values.N, e: values.E);

			return new RsaEncryptionResult(resultText, values.N, d: values.D);
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
				d = MathUtils.ModInverse(_e.Value, eulerPhi);

			return new EncryptionValues(n, eulerPhi, _e.Value, d);
		}

		private void GenerateE(int eulerPhi)
		{
			if (_e.HasValue)
				return;

			List<int> numbers = [];
			for (int i = 2; i < eulerPhi; i++)
				if (i % 2 != 0 && MathUtils.CalculateGCD(i, eulerPhi) == 1)
					numbers.Add(i);

			_e = numbers[_random.Next(numbers.Count)];
		}
	}
}