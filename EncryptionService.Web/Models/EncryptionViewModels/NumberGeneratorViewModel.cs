using System.ComponentModel.DataAnnotations;

namespace EncryptionService.Web.Models.EncryptionViewModels
{
	public class NumberGeneratorViewModel
	{
		[Display(Name = "Gamma length")]
		[Required]
		public int? GammaLength { get; set; }

		[Display(Name = "Seed")]
		[Required]
		public int? Seed { get; set; }

		public List<int> ResultNumbers { get; set; } = [];
	}
}