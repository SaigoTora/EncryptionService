using EncryptionService.Core.Interfaces;

namespace EncryptionService.Core.Models.EquivalentTransposition
{
	public class EquivalentTranspositionKey : IEncryptionKey<EquivalentTranspositionKeyData>
	{
		public required EquivalentTranspositionKeyData Key { get; init; }
	}
}