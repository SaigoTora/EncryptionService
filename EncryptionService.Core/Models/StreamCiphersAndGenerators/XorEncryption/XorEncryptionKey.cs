using EncryptionService.Core.Interfaces;

namespace EncryptionService.Core.Models.StreamCiphersAndGenerators.XorEncryption
{
	public class XorEncryptionKey : IEncryptionKey<string>
	{
		public required string Key { get; init; }
	}
}