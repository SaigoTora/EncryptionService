﻿using System.Text;

using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models.PlayfairEncryption;

namespace EncryptionService.Core.Services
{
	public class PlayfairEncryptionService
		: IEncryptionService<PlayfairEncryptionResult, PlayfairEncryptionKey, string>
	{
		private static readonly string ukrainianAlphabet = "АБВГҐДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЬЮЯ";
		private const char FILL_CHAR = '.';

		public PlayfairEncryptionResult Encrypt(string text, PlayfairEncryptionKey encryptionKey)
			=> ProcessEncryption(text, encryptionKey, true);
		public PlayfairEncryptionResult Decrypt(string encryptedText,
			PlayfairEncryptionKey encryptionKey)
			=> ProcessEncryption(encryptedText, encryptionKey, false);

		private static PlayfairEncryptionResult ProcessEncryption(string text,
			PlayfairEncryptionKey encryptionKey, bool isEncryption)
		{
			char[,] encryptionTable = CreateEncryptionTable(encryptionKey.Key);
			if (text.Length % 2 != 0)
				text += FILL_CHAR;
			text = text.ToUpper();

			StringBuilder builder = new();
			for (int i = 0; i < text.Length - 1; i += 2)
			{
				(int firstRow, int firstColumn) = FindInEncryptionTable(encryptionTable, text[i]);
				(int secondRow, int secondColumn) = FindInEncryptionTable(encryptionTable,
					text[i + 1]);

				if (firstRow == secondRow)
				{
					builder.Append(ProcessRowsEqual(encryptionTable, firstRow, firstColumn,
						isEncryption));
					builder.Append(ProcessRowsEqual(encryptionTable, secondRow, secondColumn,
						isEncryption));
				}
				else if (firstColumn == secondColumn)
				{
					builder.Append(ProcessColumnsEqual(encryptionTable, firstRow, firstColumn,
						isEncryption));
					builder.Append(ProcessColumnsEqual(encryptionTable, secondRow, secondColumn,
						isEncryption));
				}
				else
				{
					builder.Append(encryptionTable[firstRow, secondColumn]);
					builder.Append(encryptionTable[secondRow, firstColumn]);
				}
			}

			return new PlayfairEncryptionResult(builder.ToString(), encryptionTable);
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
		private static (int row, int column) FindInEncryptionTable(char[,] encryptionTable,
			char ch)
		{
			for (int i = 0; i < encryptionTable.GetLength(0); i++)
				for (int j = 0; j < encryptionTable.GetLength(1); j++)
					if (encryptionTable[i, j] == ch)
						return (i, j);

			return (-1, -1);
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