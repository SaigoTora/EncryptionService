using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models;
using EncryptionService.Core.Models.AsymmetricEncryption.KnapsackEncryption;

namespace EncryptionService.Core.Services.AsymmetricEncryption
{
	public class KnapsackEncryptionService : IEncryptionService<EncryptionResult,
		KnapsackEncryptionKey, KnapsackEncryptionKeyData>
	{
		public EncryptionResult Encrypt(string text, KnapsackEncryptionKey encryptionKey)
		{
			return new EncryptionResult($"Encrypted text. D0 = {encryptionKey.Key.D0}, " +
				$"min = {encryptionKey.Key.StepMin}, max = {encryptionKey.Key.StepMax}");
		}
		public EncryptionResult Decrypt(string encryptedText, KnapsackEncryptionKey encryptionKey)
		{
			return new EncryptionResult($"Decrypted text. D0 = {encryptionKey.Key.D0}, " +
				$"min = {encryptionKey.Key.StepMin}, max = {encryptionKey.Key.StepMax}");
		}
	}
}