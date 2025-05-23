using EncryptionService.Core.Interfaces;

namespace EncryptionService.Core.Models.AsymmetricEncryption.ElGamalEncryption
{
	public class ElGamalEncryptionKey : IEncryptionKey<ElGamalEncryptionKeyData>
	{
		public required ElGamalEncryptionKeyData Key { get; init; }
	}
}