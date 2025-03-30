using System.Text;

using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models.SloganEncryption;

namespace EncryptionService.Core.Services
{
	public class SloganEncryptionService :
		IEncryptionService<SloganEncryptionResult, SloganEncryptionKey, string>
	{
		private static readonly string ukrainianAlphabet = "АБВГҐДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЬЮЯ";
		private Dictionary<char, char> encryptionMap = [];

		public SloganEncryptionResult Encrypt(string text, SloganEncryptionKey encryptionKey)
			=> ProcessEncryption(text, encryptionKey, true);
		public SloganEncryptionResult Decrypt(string encryptedText, SloganEncryptionKey encryptionKey)
			=> ProcessEncryption(encryptedText, encryptionKey, false);

		private SloganEncryptionResult ProcessEncryption(string text,
			SloganEncryptionKey encryptionKey, bool isEncryption)
		{
			CreateEncryptionMap(encryptionKey.Key);
			text = text.ToUpper();
			string resultText = string.Empty;

			for (int i = 0; i < text.Length; i++)
			{
				if (isEncryption)
					resultText += encryptionMap[text[i]];
				else
					resultText += encryptionMap.FirstOrDefault(x => x.Value == text[i]).Key;
			}

			return new SloganEncryptionResult(resultText, encryptionMap);
		}
		private void CreateEncryptionMap(string key)
		{
			encryptionMap = [];
			char[] encryptionKeyList = new HashSet<char>(key).ToArray();
			StringBuilder tempAlphabet = new(ukrainianAlphabet);

			int k = 0;
			for (int i = 0; i < ukrainianAlphabet.Length; i++)
			{
				if (i < encryptionKeyList.Length)
				{
					encryptionMap.Add(ukrainianAlphabet[i], encryptionKeyList[i]);
					int index = tempAlphabet.ToString().IndexOf(encryptionKeyList[i]);
					tempAlphabet.Remove(index, 1);
				}
				else
					encryptionMap.Add(ukrainianAlphabet[i], tempAlphabet[k++]);
			}
		}
	}
}