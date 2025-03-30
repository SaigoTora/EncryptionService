using EncryptionService.Core.Interfaces;

namespace EncryptionService.Core.Models.PlayfairEncryption
{
	public class PlayfairEncryptionKey : IEncryptionKey<string>
	{
		public required string Key { get; init; }
	}
}