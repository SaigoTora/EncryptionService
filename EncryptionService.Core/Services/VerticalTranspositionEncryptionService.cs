using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models;

namespace EncryptionService.Core.Services
{
	public class VerticalTranspositionEncryptionService
		: IEncryptionService<VerticalTranspositionKey, string>
	{
		public EncryptionResult Encrypt(string text, VerticalTranspositionKey encryptionKey)
		{
			throw new NotImplementedException();
		}
		public EncryptionResult Decrypt(string encryptedText, VerticalTranspositionKey encryptionKey)
		{
			throw new NotImplementedException();
		}

	}
}