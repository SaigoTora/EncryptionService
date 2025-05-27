namespace EncryptionService.Core.Models.CryptoAnalysis.TranspositionAnalyzer
{
	public class TranspositionAnalyzerResult : EncryptionResult
	{
		public HashSet<int>? Indexes { get; private set; }
		public string[]? AllPermutations { get; private set; }

		public TranspositionAnalyzerResult(string text, HashSet<int> indexes)
			: base(text)
		{
			Indexes = indexes;
		}
		public TranspositionAnalyzerResult(string text, string[] allPermutations)
			: base(text)
		{
			AllPermutations = allPermutations;
		}
	}
}