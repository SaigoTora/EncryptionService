using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models;

namespace EncryptionService.Core.Services
{
	public class EquivalentTranspositionService
		: IEncryptionService<EquivalentTranspositionEncryptionResult, EquivalentTranspositionKey,
			EquivalentTranspositionKeyData>
	{
		private const char FILL_CHAR = '.';

		private static char[,]? _initialMatrix;
		private static char[,]? _transpositionMatrix;
		private static int[,]? _intialIndexes;
		private static int[]? _transpositionIndexes;

		public EquivalentTranspositionEncryptionResult Encrypt(string text,
			EquivalentTranspositionKey encryptionKey)
			=> ProcessEncryption(text, encryptionKey, true);
		public EquivalentTranspositionEncryptionResult Decrypt(string encryptedText,
			EquivalentTranspositionKey encryptionKey)
			=> ProcessEncryption(encryptedText, encryptionKey, false);

		private static EquivalentTranspositionEncryptionResult ProcessEncryption(string text,
			EquivalentTranspositionKey encryptionKey, bool isEncryption)
		{
			int rowCount = encryptionKey.Key.RowNumbers.Length;
			int columnCount = encryptionKey.Key.ColumnNumbers.Length;
			while (text.Length != rowCount * columnCount)
				text += FILL_CHAR;

			EquivalentTranspositionKeyData key = encryptionKey.Key;
			char[,] matrix = new char[rowCount, columnCount];
			_intialIndexes = new int[rowCount, columnCount];
			Direction firstDirection = isEncryption ? key.FirstWritingDirection
				: key.FirstReadingDirection;
			Direction secondDirection = isEncryption ? key.SecondWritingDirection
				: key.SecondReadingDirection;
			int k = 0;

			ProcessCells(rowCount, columnCount, firstDirection, secondDirection,
				(i, j) =>
				{
					_intialIndexes[i, j] = k;
					matrix[i, j] = text[k++];
				});
			_initialMatrix = matrix;

			matrix = TranspositionMatrix(matrix, encryptionKey.Key.RowNumbers,
				encryptionKey.Key.ColumnNumbers, isEncryption);

			_transpositionIndexes = new int[rowCount * columnCount];
			string resultText = ReadResults(matrix, key, isEncryption, rowCount, columnCount);

			return new EquivalentTranspositionEncryptionResult(resultText, _initialMatrix,
				_transpositionMatrix!, _transpositionIndexes);
		}
		private static string ReadResults(char[,] matrix,
			EquivalentTranspositionKeyData key, bool isEncryption, int rowCount, int columnCount)
		{
			string resultText = string.Empty;
			Direction firstDirection = isEncryption ? key.FirstReadingDirection
				: key.FirstWritingDirection;
			Direction secondDirection = isEncryption ? key.SecondReadingDirection
				: key.SecondWritingDirection;

			int k = 0;
			ProcessCells(rowCount, columnCount, firstDirection, secondDirection,
				(i, j) =>
				{
					_transpositionIndexes![_intialIndexes![i, j]] = k++;
					resultText += matrix[i, j];
				});

			return resultText;
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
			int[,] resultIndexes = new int[_intialIndexes!.GetLength(0),
				_intialIndexes.GetLength(1)];

			for (int i = 0; i < matrix.GetLength(0); i++)
				for (int j = 0; j < matrix.GetLength(1); j++)
				{
					int transpositionI = rowNumbers[i] - 1;
					int transpositionJ = columnNumbers[j] - 1;

					if (isEncryption)
					{
						resultIndexes[transpositionI, transpositionJ] = _intialIndexes[i, j];
						resultMatrix[transpositionI, transpositionJ] = matrix[i, j];
					}
					else
					{
						resultIndexes[i, j] = _intialIndexes[transpositionI, transpositionJ];
						resultMatrix[i, j] = matrix[transpositionI, transpositionJ];
					}
				}
			_transpositionMatrix = resultMatrix;
			_intialIndexes = resultIndexes;

			return resultMatrix;
		}
	}
}