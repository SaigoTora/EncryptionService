namespace EncryptionService.Core.Models
{
	public class EncryptionResult
	{
		public string Text { get; private set; }

		public EncryptionResult(string text)
		{
			Text = text;
		}
	}
}