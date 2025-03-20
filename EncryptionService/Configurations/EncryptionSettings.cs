using EncryptionService.Core.Models;

namespace EncryptionService.Configurations
{
	public class EncryptionSettings
	{
		public required BlockTranspositionKey BlockTranspositionKey { get; set; }
		public required VerticalTranspositionKey VerticalTranspositionKey { get; set; }
		public required EquivalentTranspositionKey EquivalentTranspositionKey { get; set; }
	}
}