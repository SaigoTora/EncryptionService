namespace EncryptionService.Core.Models.AsymmetricEncryption.ElGamalEncryption
{
	public class ElGamalEncryptionResult(string text, int y, int g, int p, int x, int a,
		List<int> b) : EncryptionResult(text)
	{
		public int Y { get; private set; } = y;
		public int G { get; private set; } = g;
		public int P { get; private set; } = p;
		public int X { get; private set; } = x;
		public int A { get; private set; } = a;
		public List<int> B { get; private set; } = b;
	}
}