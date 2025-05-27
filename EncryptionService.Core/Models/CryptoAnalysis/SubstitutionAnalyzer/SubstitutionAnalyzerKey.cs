using EncryptionService.Core.Interfaces;

namespace EncryptionService.Core.Models.CryptoAnalysis.SubstitutionAnalyzer
{
	public class SubstitutionAnalyzerKey : IEncryptionKey<Dictionary<char, char>>
	{
		public Dictionary<char, char> Key { get; init; } = [];

		public SubstitutionAnalyzerKey()
		{
			char[] ukrAlphabet =
			[
				'А', 'Б', 'В', 'Г', 'Ґ', 'Д', 'Е', 'Є', 'Ж', 'З', 'И', 'І',
				'Ї', 'Й', 'К', 'Л', 'М', 'Н', 'О', 'П', 'Р', 'С', 'Т', 'У',
				'Ф', 'Х', 'Ц', 'Ч', 'Ш', 'Щ', 'Ь', 'Ю', 'Я', ' '
			];

			char[] shuffled =
			[
				'Q', 'W', 'E', 'R', 'T', 'Y', 'U', 'I', 'O', 'P', 'A', 'S',
				'D', 'F', 'G', 'H', 'J', 'K', 'L', 'Z', 'X', 'C', 'V', 'B',
				'N', 'M', '3', '8', '1', '4', '7', '6', '5', '2', '0'
			];

			for (int i = 0; i < ukrAlphabet.Length; i++)
				Key[ukrAlphabet[i]] = shuffled[i];
		}
	}
}