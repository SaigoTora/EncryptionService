using System.Text;

using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models.CryptoAnalysis.SubstitutionAnalyzer;

namespace EncryptionService.Core.Services.CryptoAnalysis
{
	public readonly struct CharFrequency(char character, double frequency)
	{
		public char Character { get; init; } = character;
		public double Frequency { get; init; } = frequency;
	}

	public class SubstitutionAnalyzerService : ICryptoAnalyzer<SubstitutionAnalyzerResult,
		SubstitutionAnalyzerKey, SubstitutionAnalyzerKeyData>
	{
		private const char UNKNOWN_CHAR = '�';

		private static CharFrequency[]? _sortedFrequencies;
		private static CharFrequency[] SortedFrequencies
		{
			get
			{
				if (_sortedFrequencies == null)
					_sortedFrequencies = [.. LoadFrequencies().OrderBy(f => f.Frequency)];

				return _sortedFrequencies;
			}
		}

		public SubstitutionAnalyzerResult Encrypt(string text, SubstitutionAnalyzerKey key)
		{
			StringBuilder builder = new();
			foreach (char ch in text)
			{
				char upperChar = char.ToUpper(ch);

				if (!key.Key.SubstitutionMap.ContainsKey(upperChar))
				{
					builder.Append(UNKNOWN_CHAR);
					continue;
				}

				char encryptedChar = key.Key.SubstitutionMap[upperChar];

				if (char.IsLower(ch))
					builder.Append(char.ToLower(encryptedChar));
				else
					builder.Append(encryptedChar);
			}

			return new SubstitutionAnalyzerResult(builder.ToString());
		}
		public SubstitutionAnalyzerResult Decrypt(string text, SubstitutionAnalyzerKey key)
		{
			Dictionary<char, char> reverseMap = [];
			StringBuilder builder = new();

			for (int i = 0; i < text.Length; i++)
			{
				bool isLowerChar = char.IsLower(text[i]);
				char ch = char.ToUpper(text[i]);

				if (!reverseMap.ContainsKey(ch))
				{
					double frequency = text.ToUpper().Count(c => c == char.ToUpper(ch))
						/ (double)text.Length;
					reverseMap[ch] = FindEncryptedChar(frequency);
				}

				if (isLowerChar)
					builder.Append(char.ToLower(reverseMap[ch]));
				else
					builder.Append(reverseMap[ch]);
			}

			double accuracy = CalculateDecryptionAccuracy(reverseMap, key);

			return new SubstitutionAnalyzerResult(builder.ToString(), accuracy);
		}

		private static CharFrequency[] LoadFrequencies()
		{
			CharFrequency[] frequencies =
			[
				new CharFrequency('А', 0.064),
				new CharFrequency('Б', 0.013),
				new CharFrequency('В', 0.046),
				new CharFrequency('Г', 0.0129),
				new CharFrequency('Ґ', 0.0001),
				new CharFrequency('Д', 0.027),
				new CharFrequency('Е', 0.042),
				new CharFrequency('Є', 0.005),
				new CharFrequency('Ж', 0.007),
				new CharFrequency('З', 0.020),
				new CharFrequency('И', 0.055),

				new CharFrequency('І', 0.044),
				new CharFrequency('Ї', 0.010),
				new CharFrequency('Й', 0.009),
				new CharFrequency('К', 0.033),
				new CharFrequency('Л', 0.027),
				new CharFrequency('М', 0.029),
				new CharFrequency('Н', 0.068),
				new CharFrequency('О', 0.086),
				new CharFrequency('П', 0.025),
				new CharFrequency('Р', 0.043),
				new CharFrequency('С', 0.037),

				new CharFrequency('Т', 0.045),
				new CharFrequency('У', 0.027),
				new CharFrequency('Ф', 0.003),
				new CharFrequency('Х', 0.011),
				new CharFrequency('Ц', 0.010),
				new CharFrequency('Ч', 0.011),
				new CharFrequency('Ш', 0.005),
				new CharFrequency('Щ', 0.004),
				new CharFrequency('Ь', 0.016),
				new CharFrequency('Ю', 0.008),
				new CharFrequency('Я', 0.019),

				new CharFrequency(' ', 0.138),
				new CharFrequency(UNKNOWN_CHAR, 0.0)
			];

			return frequencies;
		}
		private static char FindEncryptedChar(double frequency)
		{
			double prevDiff = Double.MaxValue;

			for (int i = 0; i < SortedFrequencies.Length; i++)
			{
				double currentDiff = frequency - SortedFrequencies[i].Frequency;
				if (currentDiff < 0)
				{
					if (prevDiff < Math.Abs(currentDiff))
						return SortedFrequencies[i - 1].Character;
					return SortedFrequencies[i].Character;
				}

				prevDiff = currentDiff;
			}

			return SortedFrequencies[^1].Character;
		}
		private static double CalculateDecryptionAccuracy(Dictionary<char, char> reverseMap,
			SubstitutionAnalyzerKey key)
		{
			double totalCount = SortedFrequencies.Length;
			double correctCount = 0;

			foreach (var kvp in reverseMap)
			{
				char decryptedChar = kvp.Value;
				char originalChar = key.Key.SubstitutionMap
					.FirstOrDefault(x => x.Key == decryptedChar).Value;

				if (kvp.Key == originalChar)
					correctCount++;
			}

			double accuracy = correctCount / totalCount * 100;
			return accuracy;
		}
	}
}