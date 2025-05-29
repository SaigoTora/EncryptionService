using System.Numerics;

using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models.AsymmetricEncryption.RsaEncryption;
using EncryptionService.Core.Services.AsymmetricEncryption;
using EncryptionService.Core.Utils;

namespace EncryptionService.Core.Services.Hashing
{
	public class RsaSignatureService(IEncryptionService<RsaEncryptionResult, RsaEncryptionKey,
			RsaEncryptionKeyData> encryptionService)
		: ISignatureService<RsaEncryptionKey, RsaEncryptionKeyData>
	{
		private readonly IEncryptionService<RsaEncryptionResult, RsaEncryptionKey,
			RsaEncryptionKeyData> _encryptionService = encryptionService;

		public string CreateSignature(string text, RsaEncryptionKey key)
		{
			int n = key.Key.P * key.Key.Q;
			int hash = MathUtils.ComputeQuadraticHash(text, n);

			var encryptionResult = _encryptionService.Decrypt($"0{RsaEncryptionService.SEPARATOR}0",
				key);
			int d = encryptionResult.D
				?? throw new InvalidOperationException("Private exponent D is missing.");

			int signature = (int)BigInteger.ModPow(hash, d, n);
			return signature.ToString();
		}
		public bool VerifySignature(string text, string signature, RsaEncryptionKey key)
		{
			int n = key.Key.P * key.Key.Q;
			int expectedHash = MathUtils.ComputeQuadraticHash(text, n);

			var encryptionResult = _encryptionService.Encrypt(string.Empty, key);
			int e = encryptionResult.E
				?? throw new InvalidOperationException("Public exponent E is missing."); ;

			int s = Convert.ToInt32(signature);
			int decryptedHash = (int)BigInteger.ModPow(s, e, n);

			return decryptedHash == expectedHash;
		}
	}
}