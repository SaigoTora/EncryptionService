using EncryptionService.Core.Interfaces;

namespace EncryptionService.Core.Models.TranspositionCiphers.VerticalTransposition
{
	public class VerticalTranspositionKey : IEncryptionKey<string>
	{
		public required string Key { get; init; }
	}
}