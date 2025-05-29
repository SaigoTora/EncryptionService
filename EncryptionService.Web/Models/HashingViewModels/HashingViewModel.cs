using EncryptionService.Core.Models.Hashing;
using System.ComponentModel.DataAnnotations;

namespace EncryptionService.Web.Models.HashingViewModels
{
	public class HashingViewModel
	{
		public HashingMethod CreatingHashingMethod { get; set; }
		public HashingMethod VerifyingHashingMethod { get; set; }

		[Display(Name = "Text")]
		public string TextToHash { get; set; } = string.Empty;
		public string? GeneratedHash { get; set; }

		[Display(Name = "Verification text")]
		public string TextToVerify { get; set; } = string.Empty;
		[Display(Name = "Hash/Signature")]
		public string HashToVerify { get; set; } = string.Empty;
		public bool? VerificationResult { get; set; }
	}
}