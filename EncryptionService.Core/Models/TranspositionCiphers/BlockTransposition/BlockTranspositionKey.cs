using EncryptionService.Core.Interfaces;

namespace EncryptionService.Core.Models.TranspositionCiphers.BlockTransposition
{
	public class BlockTranspositionKey : IEncryptionKey<int[]>
	{
		public required int[] Key { get; init; }
	}
}