namespace EncryptionService.Core.Models.AsymmetricEncryption.RsaEncryption
{
	public class RsaEncryptionResult(string text, int n, int? e = null, int? d = null)
		: EncryptionResult(text)
	{
		public int N { get; private set; } = n;
		public int? E { get; private set; } = e;
		public int? D { get; private set; } = d;
	}
}