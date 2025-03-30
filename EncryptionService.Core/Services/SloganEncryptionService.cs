using System.Text;

using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models.SloganEncryption;

namespace EncryptionService.Core.Services
{
	public class SloganEncryptionService :
		IEncryptionService<SloganEncryptionResult, SloganEncryptionKey, string>
	{
		private static readonly string ukrainianAlphabet = "АБВГҐДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЬЮЯ";

		public SloganEncryptionResult Encrypt(string text, SloganEncryptionKey encryptionKey)
			=> ProcessEncryption(text, encryptionKey, true);
		public SloganEncryptionResult Decrypt(string encryptedText,
			SloganEncryptionKey encryptionKey)
			=> ProcessEncryption(encryptedText, encryptionKey, false);

		private static SloganEncryptionResult ProcessEncryption(string text,
			SloganEncryptionKey encryptionKey, bool isEncryption)
		{
			Dictionary<char, char> encryptionMap = CreateEncryptionMap(encryptionKey.Key);
			text = text.ToUpper();
			StringBuilder builder = new();

			for (int i = 0; i < text.Length; i++)
			{
				if (isEncryption)
					builder.Append(encryptionMap[text[i]]);
				else
					builder.Append(encryptionMap.FirstOrDefault(x => x.Value == text[i]).Key);
			}

			return new SloganEncryptionResult(builder.ToString(), encryptionMap);
		}
		private static Dictionary<char, char> CreateEncryptionMap(string key)
		{
			Dictionary<char, char> encryptionMap = [];
			char[] encryptionKeyArr = new HashSet<char>(key).ToArray();
			StringBuilder tempAlphabet = new(ukrainianAlphabet);

			int k = 0;
			for (int i = 0; i < ukrainianAlphabet.Length; i++)
			{
				if (i < encryptionKeyArr.Length)
				{
					encryptionMap.Add(ukrainianAlphabet[i], encryptionKeyArr[i]);
					int index = tempAlphabet.ToString().IndexOf(encryptionKeyArr[i]);
					tempAlphabet.Remove(index, 1);
				}
				else
					encryptionMap.Add(ukrainianAlphabet[i], tempAlphabet[k++]);
			}

			return encryptionMap;
		}
	}
}