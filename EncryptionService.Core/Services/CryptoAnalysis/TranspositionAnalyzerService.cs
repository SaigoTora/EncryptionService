using System.Diagnostics;
using System.Text;

using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models.CryptoAnalysis.TranspositionAnalyzer;

namespace EncryptionService.Core.Services.CryptoAnalysis
{
	public class TranspositionAnalyzerService : IEncryptionService<TranspositionAnalyzerResult,
		TranspositionAnalyzerKey, HashSet<int>>
	{
		private readonly HashSet<string> _allWords = [];
		private string? _text;

		public TranspositionAnalyzerResult Encrypt(string text,
			TranspositionAnalyzerKey encryptionKey)
		{
			StringBuilder builder = new(text.Length);
			for (int i = 0; i < encryptionKey.Key.Count; i++)
				builder.Append(text[encryptionKey.Key.ElementAt(i)]);

			return new TranspositionAnalyzerResult(builder.ToString(), encryptionKey.Key);
		}
		public TranspositionAnalyzerResult Decrypt(string encryptedText,
			TranspositionAnalyzerKey encryptionKey)
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			string[] allWords = FindAllWords(encryptedText);
			stopwatch.Stop();

			string elapsed = $"{stopwatch.Elapsed.TotalSeconds:F2} seconds";
			return new TranspositionAnalyzerResult(elapsed, allWords);
		}

		public string[] FindAllWords(string text)
		{
			_text = text;

			bool[] isLetterUsed = new bool[_text.Length];
			FindWords(string.Empty, isLetterUsed);

			return [.. _allWords];
		}
		private void FindWords(string word, bool[] isLetterUsed)
		{// Recursive method that finds all combinations
			if (word.Length == _text?.Length)
				_allWords.Add(word);

			for (int i = 0; i < _text?.Length; i++)
			{
				if (!isLetterUsed[i])
				{
					isLetterUsed[i] = true;
					FindWords(word + _text[i], isLetterUsed);
					isLetterUsed[i] = false;
				}
			}
		}
	}
}