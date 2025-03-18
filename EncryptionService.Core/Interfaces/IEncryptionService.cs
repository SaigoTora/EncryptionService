namespace EncryptionService.Core.Interfaces
{
	public interface IEncryptionService<TKey, T> where TKey : IEncryptionKey<T>
	{
		string Encrypt(string text, TKey encryptionKey);
		string Decrypt(string encryptedText, TKey encryptionKey);
	}
}