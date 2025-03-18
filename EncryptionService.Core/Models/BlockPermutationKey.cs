using EncryptionService.Core.Interfaces;

namespace EncryptionService.Core.Models
{
	public class BlockPermutationKey : IEncryptionKey<int[]>
	{
		public required int[] Key { get; init; }
	}
}