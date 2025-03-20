using EncryptionService.Core.Interfaces;

namespace EncryptionService.Core.Models
{
	public class EquivalentTranspositionService : IEncryptionService<EncryptionResult,
		EquivalentTranspositionKey, EquivalentTranspositionKeyData>
	{
		public EncryptionResult Decrypt(string encryptedText,
			EquivalentTranspositionKey encryptionKey)
		{
			throw new NotImplementedException();
		}

		public EncryptionResult Encrypt(string text, EquivalentTranspositionKey encryptionKey)
		{
			throw new NotImplementedException();
		}
	}
}