using EncryptionService.Core.Models.BlockTransposition;
using EncryptionService.Core.Models.EquivalentTransposition;
using EncryptionService.Core.Models.PlayfairEncryption;
using EncryptionService.Core.Models.SloganEncryption;
using EncryptionService.Core.Models.VerticalTransposition;

namespace EncryptionService.Configurations
{
	public class EncryptionSettings
	{
		public required BlockTranspositionKey BlockTranspositionKey { get; set; }
		public required VerticalTranspositionKey VerticalTranspositionKey { get; set; }
		public required EquivalentTranspositionKey EquivalentTranspositionKey { get; set; }
		public required SloganEncryptionKey SloganEncryptionKey { get; set; }
		public required PlayfairEncryptionKey PlayfairEncryptionKey { get; set; }
		public required Dictionary<string, int> HomophonicEncryptionFrequency { get; set; }
	}
}