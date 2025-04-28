using EncryptionService.Core.Interfaces;

namespace EncryptionService.Core.Models.SubstitutionCiphers.SloganEncryption
{
	public class SloganEncryptionKey : IEncryptionKey<string>
	{
		public required string Key { get; init; }
	}
}