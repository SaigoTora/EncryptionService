using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models;
using EncryptionService.Core.Models.SloganEncryption;

namespace EncryptionService.Core.Services
{
	public class SloganEncryptionService :
		IEncryptionService<EncryptionResult, SloganEncryptionKey, string>
	{
		public EncryptionResult Encrypt(string text, SloganEncryptionKey encryptionKey)
		{
			return new EncryptionResult("encrypted text");
		}
		public EncryptionResult Decrypt(string encryptedText, SloganEncryptionKey encryptionKey)
		{
			return new EncryptionResult("decrypted text");
		}
	}
}