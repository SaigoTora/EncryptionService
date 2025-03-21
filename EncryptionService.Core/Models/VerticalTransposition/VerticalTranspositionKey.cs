using EncryptionService.Core.Interfaces;

namespace EncryptionService.Core.Models.VerticalTransposition
{
	public class VerticalTranspositionKey : IEncryptionKey<string>
	{
		public required string Key { get; init; }
	}
}