using EncryptionService.Core.Interfaces;

namespace EncryptionService.Core.Models.AsymmetricEncryption.KnapsackEncryption
{
	public class KnapsackEncryptionKey : IEncryptionKey<KnapsackEncryptionKeyData>
	{
		public required KnapsackEncryptionKeyData Key { get; init; }
	}
}