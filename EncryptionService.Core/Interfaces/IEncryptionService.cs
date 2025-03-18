using EncryptionService.Core.Models;

namespace EncryptionService.Core.Interfaces
{
	public interface IEncryptionService<TKey, T> where TKey : IEncryptionKey<T>
	{
		EncryptionResult Encrypt(string text, TKey encryptionKey);
		EncryptionResult Decrypt(string encryptedText, TKey encryptionKey);
	}
}