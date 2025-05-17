namespace EncryptionService.Core.Models.StreamCiphersAndGenerators.LfsrGenerator
{
	public class LfsrEncryptionResult(string text, string binaryText, string hexText)
		: EncryptionResult(text)
	{
		public string BinaryText { get; private set; } = binaryText;
		public string HexText { get; private set; } = hexText;
	}
}