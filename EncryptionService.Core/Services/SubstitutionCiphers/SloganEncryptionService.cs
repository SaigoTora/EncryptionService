using System.Text;

using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models.SubstitutionCiphers.SloganEncryption;

namespace EncryptionService.Core.Services.SubstitutionCiphers
{
	public class SloganEncryptionService :
		IEncryptionService<SloganEncryptionResult, SloganEncryptionKey, string>
	{
		private const char UNKNOWN_CHAR = '�';
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
				{
					if (encryptionMap.TryGetValue(text[i], out char value))
						builder.Append(value);
					else
						builder.Append(UNKNOWN_CHAR);
				}
				else
				{
					char ch = encryptionMap.FirstOrDefault(x => x.Value == text[i]).Key;
					if (ch != default)
						builder.Append(ch);
					else
						builder.Append(UNKNOWN_CHAR);
				}
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