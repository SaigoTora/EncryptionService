using EncryptionService.Core.Interfaces;

namespace EncryptionService.Core.Models.StreamCiphersAndGenerators.IcgGenerator
{
	public class IcgGeneratorParameters : IGeneratorParameters
	{
		public required int A { get; init; }
		public required int B { get; init; }
		public required int M { get; init; }
	}
}