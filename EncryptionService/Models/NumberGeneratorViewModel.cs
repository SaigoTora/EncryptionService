using System.ComponentModel.DataAnnotations;

namespace EncryptionService.Models
{
	public class NumberGeneratorViewModel
	{
		[Required]
		public int Seed { get; set; }
		[Required]
		public int GammaLength { get; set; }
		public List<int> ResultNumbers { get; set; } = [];
	}
}