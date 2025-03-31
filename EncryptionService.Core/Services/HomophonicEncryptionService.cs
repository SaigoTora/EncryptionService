using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models;
using EncryptionService.Core.Models.HomophonicEncryption;

namespace EncryptionService.Core.Services
{
	public class HomophonicEncryptionService
		: IEncryptionService<EncryptionResult, HomophonicEncryptionKey,
			Dictionary<char, int[]>>
	{
		public EncryptionResult Encrypt(string text, HomophonicEncryptionKey encryptionKey)
		{
			return new EncryptionResult("encrypted text");
		}
		public EncryptionResult Decrypt(string encryptedText, HomophonicEncryptionKey encryptionKey)
		{
			return new EncryptionResult("decrypted text");
		}
	}
}