namespace EncryptionService.Core.Models.HomophonicEncryption
{
	public class HomophonicEncryptionResult(string text, Dictionary<char, int[]> encryptionTable)
		: EncryptionResult(text)
	{
		public Dictionary<char, int[]> EncryptionTable { get; private set; } = encryptionTable;
	}
}