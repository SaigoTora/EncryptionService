using EncryptionService.Core.Models.BlockTransposition;
using EncryptionService.Core.Models.EquivalentTransposition;
using EncryptionService.Core.Models.IcgGenerator;
using EncryptionService.Core.Models.PlayfairEncryption;
using EncryptionService.Core.Models.SloganEncryption;
using EncryptionService.Core.Models.VerticalTransposition;
using EncryptionService.Core.Models.XorEncryption;

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
		public required XorEncryptionKey XorEncryptionKey { get; set; }
		public required IcgGeneratorParameters IcgGeneratorParameters { get; set; }
	}
}