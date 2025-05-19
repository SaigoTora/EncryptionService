using EncryptionService.Core.Models;

namespace EncryptionService.Web.Models.EncryptionViewModels
{
	public class FileEncryptionViewModel<TEncryptionResult>
		: EncryptionViewModel<TEncryptionResult> where TEncryptionResult : EncryptionResult
	{
		public IFormFile? EncryptionInputFile { get; set; }
		public IFormFile? DecryptionInputFile { get; set; }
	}
}