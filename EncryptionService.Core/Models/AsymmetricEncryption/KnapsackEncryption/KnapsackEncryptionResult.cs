namespace EncryptionService.Core.Models.AsymmetricEncryption.KnapsackEncryption
{
	public class KnapsackEncryptionResult(string text, int n, int m, int[] d, int[] e)
		: EncryptionResult(text)
	{
		public int N { get; private set; } = n;
		public int M { get; private set; } = m;
		public int[] D { get; private set; } = d;
		public int[] E { get; private set; } = e;
	}
}