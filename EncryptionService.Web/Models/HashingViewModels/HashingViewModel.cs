using EncryptionService.Core.Models.Hashing;

namespace EncryptionService.Web.Models.HashingViewModels
{
	public class HashingViewModel
	{
		public HashingMethod HashingMethod { get; set; }

		public string? TextToHash { get; set; }
		public string? GeneratedHash { get; set; }

		public string? TextToVerify { get; set; }
		public string? HashToVerify { get; set; }
		public bool? VerificationResult { get; set; }
	}
}
