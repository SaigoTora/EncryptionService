using EncryptionService.Core.Models;

namespace EncryptionService.Core.Interfaces
{
	public interface IEncryptionService<TEncryptionResult, TEncryptionKey, TKeyData>
		where TEncryptionResult : EncryptionResult
		where TEncryptionKey : IEncryptionKey<TKeyData>
	{
		TEncryptionResult Encrypt(string text, TEncryptionKey encryptionKey);
		TEncryptionResult Decrypt(string encryptedText, TEncryptionKey encryptionKey);
	}
}