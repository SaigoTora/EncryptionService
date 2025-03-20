namespace EncryptionService.Core.Models
{
	public class EncryptionResult(string text)
	{
		public string Text { get; private set; } = text;
	}
}