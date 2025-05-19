using EncryptionService.Core.Models;

namespace EncryptionService.Web.Models.EncryptionViewModels
{
	public class EncryptionViewModel<TEncryptionResult> where TEncryptionResult : EncryptionResult
	{
		public string? InputText { get; set; }
		public TEncryptionResult? EncryptionResult { get; set; }
		public string? EncryptedInputText { get; set; }
		public TEncryptionResult? DecryptionResult { get; set; }
	}
}