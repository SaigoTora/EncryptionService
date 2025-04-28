namespace EncryptionService.Core.Models.SubstitutionCiphers.PlayfairEncryption
{
	public class PlayfairEncryptionResult(string text, char[,] encryptionTable)
		: EncryptionResult(text)
	{
		public char[,] EncryptionTable { get; private set; } = encryptionTable;
	}
}