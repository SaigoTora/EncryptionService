namespace EncryptionService.Core.Models.AsymmetricEncryption.RsaEncryption
{
	public class RsaEncryptionKeyData
	{
		public required int P { get; init; }
		public required int Q { get; init; }
	}
}