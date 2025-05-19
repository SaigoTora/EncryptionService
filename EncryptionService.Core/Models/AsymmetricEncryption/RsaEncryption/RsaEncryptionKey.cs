using EncryptionService.Core.Interfaces;

namespace EncryptionService.Core.Models.AsymmetricEncryption.RsaEncryption
{
	public class RsaEncryptionKey : IEncryptionKey<RsaEncryptionKeyData>
	{
		public required RsaEncryptionKeyData Key { get; init; }
	}
}