namespace EncryptionService.Core.Interfaces
{
	public interface ISignatureService<TKey, TKeyData> where TKey : IEncryptionKey<TKeyData>
	{
		string CreateSignature(string text, TKey key);
		bool VerifySignature(string text, string signature, TKey key);
	}
}