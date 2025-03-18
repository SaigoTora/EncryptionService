using EncryptionService.Core.Models;

namespace EncryptionService.Configurations
{
	public class EncryptionSettings
	{
		public required BlockPermutationKey BlockPermutationKey { get; set; }
	}
}