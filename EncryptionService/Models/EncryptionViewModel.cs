using EncryptionService.Core.Models;
using EncryptionService.Models.Attributes;

namespace EncryptionService.Models
{
	public class EncryptionViewModel<TEncryptionResult> where TEncryptionResult : EncryptionResult
	{
		[AtLeastOneFieldRequired("EncryptedInputText",
			"At least one of the fields must be filled in.")]
		public string? InputText { get; set; }
		public TEncryptionResult? EncryptionResult { get; set; }
		[AtLeastOneFieldRequired("InputText")]
		public string? EncryptedInputText { get; set; }
		public TEncryptionResult? DecryptionResult { get; set; }
	}
}