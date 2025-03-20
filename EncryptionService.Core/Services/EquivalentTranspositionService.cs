using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models;

namespace EncryptionService.Core.Services
{
	public class EquivalentTranspositionService : IEncryptionService<EncryptionResult,
		EquivalentTranspositionKey, EquivalentTranspositionKeyData>
	{
		private const char FILL_CHAR = '.';

		public EncryptionResult Encrypt(string text, EquivalentTranspositionKey encryptionKey)
			=> ProcessEncryption(text, encryptionKey, true);
		public EncryptionResult Decrypt(string encryptedText,
			EquivalentTranspositionKey encryptionKey)
			=> ProcessEncryption(encryptedText, encryptionKey, false);

		private static EncryptionResult ProcessEncryption(string text,
			EquivalentTranspositionKey encryptionKey, bool isEncryption)
		{
			while (text.Length != encryptionKey.Key.RowCount * encryptionKey.Key.ColumnCount)
				text += FILL_CHAR;

			EquivalentTranspositionKeyData key = encryptionKey.Key;
			char[,] matrix = new char[key.RowCount, key.ColumnCount];
			Direction firstDirection = isEncryption ? key.FirstWritingDirection
				: key.FirstReadingDirection;
			Direction secondDirection = isEncryption ? key.SecondWritingDirection
				: key.SecondReadingDirection;
			int k = 0;

			ProcessCells(key.RowCount, key.ColumnCount, firstDirection, secondDirection,
				(i, j) => matrix[i, j] = text[k++]);

			matrix = TranspositionMatrix(matrix, encryptionKey.Key.RowNumbers,
				encryptionKey.Key.ColumnNumbers, isEncryption);

			string resultText = string.Empty;
			firstDirection = isEncryption ? key.FirstReadingDirection
				: key.FirstWritingDirection;
			secondDirection = isEncryption ? key.SecondReadingDirection
				: key.SecondWritingDirection;
			ProcessCells(key.RowCount, key.ColumnCount, firstDirection, secondDirection,
				(i, j) => resultText += matrix[i, j]);

			return new EncryptionResult(resultText);
		}
		private static void ProcessCells(int rowCount, int columnCount,
			Direction firstDirection, Direction secondDirection,
			Action<int, int> processCell)
		{
			(int firstStart, int firstEnd, int firstStep) = GetDirectionRange(firstDirection,
				rowCount, columnCount);
			(int secondStart, int secondEnd, int secondStep) = GetDirectionRange(secondDirection,
				rowCount, columnCount);

			for (int i = firstStart; i != firstEnd; i += firstStep)
				for (int j = secondStart; j != secondEnd; j += secondStep)
				{
					if (firstDirection == Direction.Left || firstDirection == Direction.Right)
						processCell(i, j);
					else
						processCell(j, i);
				}
		}
		private static (int start, int end, int step) GetDirectionRange(Direction direction,
			int rowCount, int columnCount)
		{
			int start, end, step;
			if (direction == Direction.Left || direction == Direction.Right)
			{
				start = direction == Direction.Right ? 0 : columnCount - 1;
				end = direction == Direction.Right ? columnCount : -1;
			}
			else
			{
				start = direction == Direction.Down ? 0 : rowCount - 1;
				end = direction == Direction.Down ? rowCount : -1;
			}
			step = start < end ? 1 : -1;

			return (start, end, step);
		}
		private static char[,] TranspositionMatrix(char[,] matrix, int[] rowNumbers,
			int[] columnNumbers, bool isEncryption)
		{
			char[,] resultMatrix = new char[matrix.GetLength(0), matrix.GetLength(1)];

			for (int i = 0; i < matrix.GetLength(0); i++)
				for (int j = 0; j < matrix.GetLength(1); j++)
				{
					if (isEncryption)
						resultMatrix[rowNumbers[i] - 1, columnNumbers[j] - 1] = matrix[i, j];
					else
						resultMatrix[i, j] = matrix[rowNumbers[i] - 1, columnNumbers[j] - 1];
				}

			return resultMatrix;
		}
	}
}