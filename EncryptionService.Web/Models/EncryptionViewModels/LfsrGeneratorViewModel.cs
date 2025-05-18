using EncryptionService.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace EncryptionService.Web.Models.EncryptionViewModels
{
	public class LfsrGeneratorViewModel<TEncryptionResult>
		: EncryptionViewModel<TEncryptionResult> where TEncryptionResult : EncryptionResult
	{
		[Required(ErrorMessage = "The \"Initial state\" field must be filled in.")]
		public string InitialState { get; set; } = string.Empty;
		public EncryptionFormat EncryptionFormat { get; set; }
		public EncryptionFormat DecryptionFormat { get; set; }
	}
}