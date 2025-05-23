using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models;
using EncryptionService.Core.Models.AsymmetricEncryption.ElGamalEncryption;

namespace EncryptionService.Core.Services.AsymmetricEncryption
{
	public class ElGamalEncryptionService : IEncryptionService<EncryptionResult,
		ElGamalEncryptionKey, ElGamalEncryptionKeyData>
	{
		public EncryptionResult Encrypt(string text, ElGamalEncryptionKey encryptionKey)
		{
			return new EncryptionResult($"Encrypted text p = {encryptionKey.Key.P}, " +
				$"k = {encryptionKey.Key.K}");
		}
		public EncryptionResult Decrypt(string encryptedText, ElGamalEncryptionKey encryptionKey)
		{
			return new EncryptionResult($"Decrypted text p = {encryptionKey.Key.P}, " +
				$"k = {encryptionKey.Key.K}");
		}
	}
}