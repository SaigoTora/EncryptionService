using EncryptionService.Core.Interfaces;

namespace EncryptionService.Core.Models
{
	public class EquivalentTranspositionKey : IEncryptionKey<EquivalentTranspositionKeyData>
	{
		public required EquivalentTranspositionKeyData Key { get; init; }
	}
}