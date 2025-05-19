using EncryptionService.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace EncryptionService.Web.Models.EncryptionViewModels
{
	public class LfsrEncryptionViewModel<TEncryptionResult>
		: EncryptionViewModel<TEncryptionResult> where TEncryptionResult : EncryptionResult
	{
		[Required(ErrorMessage = "Initial state is required.")]
		public string InitialState { get; set; } = string.Empty;
		public EncryptionFormat EncryptionFormat { get; set; }
		public EncryptionFormat DecryptionFormat { get; set; }
		public IFormFile? EncryptionInputFile { get; set; }
		public IFormFile? DecryptionInputFile { get; set; }
	}
}