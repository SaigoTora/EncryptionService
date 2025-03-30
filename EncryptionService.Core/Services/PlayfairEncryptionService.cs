using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models;
using EncryptionService.Core.Models.PlayfairEncryption;

namespace EncryptionService.Core.Services
{
	public class PlayfairEncryptionService
		: IEncryptionService<EncryptionResult, PlayfairEncryptionKey, string>
	{
		private const char FILL_CHAR = '.';

		public EncryptionResult Encrypt(string text, PlayfairEncryptionKey encryptionKey)
		{
			return new EncryptionResult($"encrypted text {encryptionKey.Key}");
		}
		public EncryptionResult Decrypt(string encryptedText, PlayfairEncryptionKey encryptionKey)
		{
			return new EncryptionResult("decrypted text");
		}
	}
}