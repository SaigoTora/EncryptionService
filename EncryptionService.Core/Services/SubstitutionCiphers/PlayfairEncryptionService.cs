using System.Text;

using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models.SubstitutionCiphers.PlayfairEncryption;

namespace EncryptionService.Core.Services.SubstitutionCiphers
{
	public class PlayfairEncryptionService
		: IEncryptionService<PlayfairEncryptionResult, PlayfairEncryptionKey, string>
	{
		private const char FILL_CHAR = '.';
		private const char UNKNOWN_CHAR = '�';
		private const char ADDITIONAL_CHAR = 'Х';
		private static readonly string ukrainianAlphabet = "АБВГҐДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЬЮЯ";

		public PlayfairEncryptionResult Encrypt(string text, PlayfairEncryptionKey encryptionKey)
			=> ProcessEncryption(text, encryptionKey, true);
		public PlayfairEncryptionResult Decrypt(string encryptedText,
			PlayfairEncryptionKey encryptionKey)
			=> ProcessEncryption(encryptedText, encryptionKey, false);

		private static PlayfairEncryptionResult ProcessEncryption(string text,
			PlayfairEncryptionKey encryptionKey, bool isEncryption)
		{
			char[,] encryptionTable = CreateEncryptionTable(encryptionKey.Key);
			text = text.ToUpper();

			if (isEncryption)
				for (int i = 0; i < text.Length - 1; i += 2)
					if (text[i] == text[i + 1])
						text = text.Insert(i + 1, ADDITIONAL_CHAR.ToString());

			if (text.Length % 2 != 0)
				text += FILL_CHAR;

			string resultText = ProcessText(text, encryptionTable, isEncryption);

			return new PlayfairEncryptionResult(resultText, encryptionTable);
		}

		private static char[,] CreateEncryptionTable(string key)
		{
			int size = (int)Math.Round(Math.Sqrt(ukrainianAlphabet.Length));
			char[,] encryptionTable = new char[size, size];
			List<char> encryptionKeyList = new HashSet<char>(key).ToList();
			StringBuilder tempAlphabet = new(ukrainianAlphabet);

			int k = 0;
			for (int i = 0; i < encryptionTable.GetLength(0); i++)
				for (int j = 0; j < encryptionTable.GetLength(1); j++)
				{
					if (encryptionKeyList.Count > 0)
					{
						encryptionTable[i, j] = encryptionKeyList[0];
						int index = tempAlphabet.ToString().IndexOf(encryptionKeyList[0]);
						encryptionKeyList.RemoveAt(0);
						tempAlphabet.Remove(index, 1);
					}
					else
					{
						if (k < tempAlphabet.Length)
							encryptionTable[i, j] = tempAlphabet[k++];
						else
							encryptionTable[i, j] = FILL_CHAR;

					}
				}

			return encryptionTable;
		}
		private static (int? row, int? column) FindInEncryptionTable(char[,] encryptionTable,
			char ch)
		{
			for (int i = 0; i < encryptionTable.GetLength(0); i++)
				for (int j = 0; j < encryptionTable.GetLength(1); j++)
					if (encryptionTable[i, j] == ch)
						return (i, j);

			return (null, null);
		}

		private static string ProcessText(string text, char[,] encryptionTable, bool isEncryption)
		{
			StringBuilder builder = new();
			for (int i = 0; i < text.Length - 1; i += 2)
			{
				(int? firstRow, int? firstColumn) = FindInEncryptionTable(encryptionTable,
					text[i]);
				(int? secondRow, int? secondColumn) = FindInEncryptionTable(encryptionTable,
					text[i + 1]);

				HandleEncryption(builder, encryptionTable, firstRow, firstColumn,
					secondRow, secondColumn, isEncryption);
			}

			return builder.ToString();
		}
		private static void HandleEncryption(StringBuilder builder, char[,] encryptionTable,
			int? firstRow, int? firstColumn, int? secondRow, int? secondColumn, bool isEncryption)
		{
			if (firstRow.HasValue && firstColumn.HasValue
				&& secondRow.HasValue && secondColumn.HasValue)
			{
				if (firstRow == secondRow)
				{
					builder.Append(ProcessRowsEqual(encryptionTable, firstRow.Value,
						firstColumn.Value, isEncryption));
					builder.Append(ProcessRowsEqual(encryptionTable, secondRow.Value,
						secondColumn.Value, isEncryption));
				}
				else if (firstColumn == secondColumn)
				{
					builder.Append(ProcessColumnsEqual(encryptionTable, firstRow.Value,
						firstColumn.Value, isEncryption));
					builder.Append(ProcessColumnsEqual(encryptionTable, secondRow.Value,
						secondColumn.Value, isEncryption));
				}
				else
				{
					builder.Append(encryptionTable[firstRow.Value, secondColumn.Value]);
					builder.Append(encryptionTable[secondRow.Value, firstColumn.Value]);
				}
			}
			else
				HandleUnknownCharacters(builder, encryptionTable, firstRow, firstColumn,
					secondRow, secondColumn);
		}
		private static void HandleUnknownCharacters(StringBuilder builder, char[,] encryptionTable,
			int? firstRow, int? firstColumn, int? secondRow, int? secondColumn)
		{
			if (firstRow == null && firstColumn == null
					&& secondRow == null && secondColumn == null)
			{
				builder.Append(UNKNOWN_CHAR);
				builder.Append(UNKNOWN_CHAR);
			}
			else if (firstRow == null && firstColumn == null
				&& secondRow.HasValue && secondColumn.HasValue)
			{
				builder.Append(UNKNOWN_CHAR);
				builder.Append(encryptionTable[secondRow.Value, secondColumn.Value]);
			}
			else if (firstRow.HasValue && firstColumn.HasValue
				&& secondRow == null && secondColumn == null)
			{
				builder.Append(encryptionTable[firstRow.Value, firstColumn.Value]);
				builder.Append(UNKNOWN_CHAR);
			}
		}

		private static char ProcessRowsEqual(char[,] encryptionTable, int row, int column,
			bool isEncryption)
		{
			if (isEncryption)
				return GetRightSymbol(encryptionTable, row, column);
			else
				return GetLeftSymbol(encryptionTable, row, column);
		}
		private static char GetRightSymbol(char[,] encryptionTable, int row, int column)
		{
			if (column == encryptionTable.GetLength(1) - 1)
				return encryptionTable[row, 0];
			else
				return encryptionTable[row, column + 1];
		}
		private static char GetLeftSymbol(char[,] encryptionTable, int row, int column)
		{
			if (column == 0)
				return encryptionTable[row, encryptionTable.GetLength(1) - 1];
			else
				return encryptionTable[row, column - 1];
		}

		private static char ProcessColumnsEqual(char[,] encryptionTable, int row, int column,
			bool isEncryption)
		{
			if (isEncryption)
				return GetDownSymbol(encryptionTable, row, column);
			else
				return GetUpSymbol(encryptionTable, row, column);
		}
		private static char GetDownSymbol(char[,] encryptionTable, int row, int column)
		{
			if (row == encryptionTable.GetLength(0) - 1)
				return encryptionTable[0, column];
			else
				return encryptionTable[row + 1, column];
		}
		private static char GetUpSymbol(char[,] encryptionTable, int row, int column)
		{
			if (row == 0)
				return encryptionTable[encryptionTable.GetLength(0) - 1, column];
			else
				return encryptionTable[row - 1, column];
		}
	}
}