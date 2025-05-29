using System.ComponentModel.DataAnnotations;

using EncryptionService.Core.Models;

namespace EncryptionService.Web.Models.EncryptionViewModels
{
	public class EncryptionViewModel<TEncryptionResult> where TEncryptionResult : EncryptionResult
	{
		[Display(Name = "Text")]
		public string InputText { get; set; } = string.Empty;
		public TEncryptionResult? EncryptionResult { get; set; }

		[Display(Name = "Encrypted text")]
		public string EncryptedInputText { get; set; } = string.Empty;
		public TEncryptionResult? DecryptionResult { get; set; }
	}
}