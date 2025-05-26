using EncryptionService.Core.Interfaces;

namespace EncryptionService.Core.Models.CryptoAnalysis.SubstitutionAnalyzer
{
	public class SubstitutionAnalyzerKey : IEncryptionKey<SubstitutionAnalyzerKeyData>
	{
		public SubstitutionAnalyzerKeyData Key { get; init; } = new();
	}
}