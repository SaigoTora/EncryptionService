namespace EncryptionService.Core.Models.CryptoAnalysis.SubstitutionAnalyzer
{
	public class SubstitutionAnalyzerResult : EncryptionResult
	{
		public double? Accuracy { get; private set; }

		public SubstitutionAnalyzerResult(string text)
			: base(text)
		{ }
		public SubstitutionAnalyzerResult(string text, double accuracy)
			: base(text)
		{
			Accuracy = accuracy;
		}
	}
}