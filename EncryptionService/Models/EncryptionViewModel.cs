using EncryptionService.Models.Attributes;

namespace EncryptionService.Models
{
	public class EncryptionViewModel
	{
		[AtLeastOneFieldRequired("EncryptedInputText", "At least one of the fields must be filled in.")]
		public string? InputText { get; set; }
		public string? EncryptedText { get; set; }
		[AtLeastOneFieldRequired("InputText")]
		public string? EncryptedInputText { get; set; }
		public string? DecryptedText { get; set; }
	}
}