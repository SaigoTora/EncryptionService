namespace EncryptionService.Core.Models.AsymmetricEncryption.ElGamalEncryption
{
	public class ElGamalEncryptionKeyData
	{
		public required int P { get; init; }
		public required int K { get; init; }
	}
}