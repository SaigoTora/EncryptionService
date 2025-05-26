using EncryptionService.Core.Models;

namespace EncryptionService.Core.Interfaces
{
	public interface ICryptoAnalyzer<TEncryptionResult, TEncryptionKey, TKeyData>
		where TEncryptionResult : EncryptionResult
		where TEncryptionKey : IEncryptionKey<TKeyData>
	{
		TEncryptionResult Encrypt(string text, TEncryptionKey key);
		TEncryptionResult Decrypt(string text, TEncryptionKey key);
	}
}