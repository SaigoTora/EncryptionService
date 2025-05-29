using System.Numerics;

using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models.AsymmetricEncryption.ElGamalEncryption;
using EncryptionService.Core.Utils;

namespace EncryptionService.Core.Services.Hashing
{
	public class ElGamalSignatureService(IEncryptionService<ElGamalEncryptionResult,
		ElGamalEncryptionKey, ElGamalEncryptionKeyData> encryptionService)
		: ISignatureService<ElGamalEncryptionKey, ElGamalEncryptionKeyData>
	{
		public const char SEPARATOR = ',';
		private readonly IEncryptionService<ElGamalEncryptionResult, ElGamalEncryptionKey,
			ElGamalEncryptionKeyData> _encryptionService = encryptionService;

		public string CreateSignature(string text, ElGamalEncryptionKey key)
		{
			int p = key.Key.P;
			var encryptionResult = _encryptionService.Encrypt(string.Empty, key);
			int g = encryptionResult.G;
			int x = encryptionResult.X;

			int k = key.Key.K;

			int m = MathUtils.ComputeQuadraticHash(text, p);
			int kInv = MathUtils.ModInverse(k, p - 1);
			int a = (int)BigInteger.ModPow(g, k, p);
			int b = (kInv * (m - x * a)) % (p - 1);

			if (b < 0)
				b += (p - 1);

			return $"{a}{SEPARATOR}{b}";
		}
		public bool VerifySignature(string text, string signature, ElGamalEncryptionKey key)
		{
			int p = key.Key.P;
			int m = MathUtils.ComputeQuadraticHash(text, p);

			var encryptionResult = _encryptionService.Encrypt(string.Empty, key);
			int g = encryptionResult.G;
			int y = encryptionResult.Y;

			var parts = signature.Split(SEPARATOR);
			int a = Convert.ToInt32(parts[0]);
			int b = Convert.ToInt32(parts[1]);

			if (a <= 0 || a >= p)
				return false;

			BigInteger left = BigInteger.ModPow(g, m, p);
			BigInteger right = (BigInteger.ModPow(y, a, p) * BigInteger.ModPow(a, b, p)) % p;

			return left == right;
		}
	}
}