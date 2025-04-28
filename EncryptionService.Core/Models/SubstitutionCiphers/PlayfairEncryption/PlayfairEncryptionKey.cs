using EncryptionService.Core.Interfaces;

namespace EncryptionService.Core.Models.SubstitutionCiphers.PlayfairEncryption
{
	public class PlayfairEncryptionKey : IEncryptionKey<string>
	{
		public required string Key { get; init; }
	}
}