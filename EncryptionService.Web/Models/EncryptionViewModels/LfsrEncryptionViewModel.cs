using System.ComponentModel.DataAnnotations;

using EncryptionService.Core.Models;

namespace EncryptionService.Web.Models.EncryptionViewModels
{
	public class LfsrEncryptionViewModel<TEncryptionResult>
		: FileEncryptionViewModel<TEncryptionResult> where TEncryptionResult : EncryptionResult
	{
		[Required(ErrorMessage = "Initial state is required.")]
		public string InitialState { get; set; } = string.Empty;
		public EncryptionFormat EncryptionFormat { get; set; }
		public EncryptionFormat DecryptionFormat { get; set; }
	}
}