using System.Text;
using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models;
using EncryptionService.Core.Models.StreamCiphersAndGenerators.XorEncryption;

namespace EncryptionService.Core.Services.StreamCiphersAndGenerators
{
	public class XorEncryptionService : IEncryptionService<EncryptionResult,
		XorEncryptionKey, string>
	{
		public EncryptionResult Encrypt(string text, XorEncryptionKey encryptionKey)
			=> ProcessEncryption(text, encryptionKey);
		public EncryptionResult Decrypt(string encryptedText, XorEncryptionKey encryptionKey)
			=> ProcessEncryption(encryptedText, encryptionKey);

		private static EncryptionResult ProcessEncryption(string text,
			XorEncryptionKey encryptionKey)
		{
			string key = encryptionKey.Key;

			StringBuilder builder = new();
			for (int i = 0; i < text.Length; i++)
				builder.Append((char)(text[i] ^ key[i % key.Length]));

			return new EncryptionResult(builder.ToString());
		}
	}
}