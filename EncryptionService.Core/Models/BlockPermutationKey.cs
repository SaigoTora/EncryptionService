using EncryptionService.Core.Interfaces;

namespace EncryptionService.Core.Models
{
	public class BlockTranspositionKey : IEncryptionKey<int[]>
	{
		public required int[] Key { get; init; }
	}
}