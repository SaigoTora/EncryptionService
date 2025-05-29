using System.ComponentModel.DataAnnotations;

using EncryptionService.Core.Models;

namespace EncryptionService.Web.Models.EncryptionViewModels
{
	public class LfsrEncryptionViewModel<TEncryptionResult>
		: FileEncryptionViewModel<TEncryptionResult> where TEncryptionResult : EncryptionResult
	{
		[Display(Name = "Encryption initial state")]
		public string EncryptionInitialState { get; set; } = string.Empty;

		[Display(Name = "Decryption initial state")]
		public string DecryptionInitialState { get; set; } = string.Empty;
		public EncryptionFormat EncryptionFormat { get; set; }
		public EncryptionFormat DecryptionFormat { get; set; }
	}
}