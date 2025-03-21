using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models;

namespace EncryptionService.Core.Services
{
	public class EquivalentTranspositionService
		: IEncryptionService<EquivalentTranspositionEncryptionResult, EquivalentTranspositionKey,
			EquivalentTranspositionKeyData>
	{
		private const char FILL_CHAR = '.';

		private static int _rowCount;
		private static int _columnCount;

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
			_rowCount = encryptionKey.Key.RowNumbers.Length;
			_columnCount = encryptionKey.Key.ColumnNumbers.Length;
			while (text.Length != _rowCount * _columnCount)
				text += FILL_CHAR;

			EquivalentTranspositionKeyData key = encryptionKey.Key;
			char[,] matrix = new char[_rowCount, _columnCount];
			_intialIndexes = new int[_rowCount, _columnCount];
			Direction firstDirection = isEncryption ? key.FirstWritingDirection
				: key.FirstReadingDirection;
			Direction secondDirection = isEncryption ? key.SecondWritingDirection
				: key.SecondReadingDirection;
			int k = 0;

			ProcessCells(firstDirection, secondDirection,
				(i, j) =>
				{
					_intialIndexes[i, j] = k;
					matrix[i, j] = text[k++];
				});
			_initialMatrix = matrix;

			matrix = TranspositionMatrix(matrix, encryptionKey.Key.RowNumbers,
				encryptionKey.Key.ColumnNumbers, isEncryption);

			_transpositionIndexes = new int[_rowCount * _columnCount];
			string resultText = ReadResults(matrix, key, isEncryption);

			return new EquivalentTranspositionEncryptionResult(resultText, _initialMatrix,
				_transpositionMatrix!, _transpositionIndexes);
		}
		private static string ReadResults(char[,] matrix,
			EquivalentTranspositionKeyData key, bool isEncryption)
		{
			string resultText = string.Empty;
			Direction firstDirection = isEncryption ? key.FirstReadingDirection
				: key.FirstWritingDirection;
			Direction secondDirection = isEncryption ? key.SecondReadingDirection
				: key.SecondWritingDirection;

			int k = 0;
			ProcessCells(firstDirection, secondDirection,
				(i, j) =>
				{
					_transpositionIndexes![_intialIndexes![i, j]] = k++;
					resultText += matrix[i, j];
				});

			return resultText;
		}

		private static void ProcessCells(Direction firstDirection, Direction secondDirection,
			Action<int, int> processCell)
		{
			(int firstStart, int firstEnd, int firstStep) = GetDirectionRange(firstDirection);
			(int secondStart, int secondEnd, int secondStep) = GetDirectionRange(secondDirection);

			for (int i = secondStart; i != secondEnd; i += secondStep)
				for (int j = firstStart; j != firstEnd; j += firstStep)
				{
					if (firstDirection == Direction.Left || firstDirection == Direction.Right)
						processCell(i, j);
					else
						processCell(j, i);
				}
		}
		private static (int start, int end, int step) GetDirectionRange(Direction direction)
		{
			int start, end, step;
			if (direction == Direction.Left || direction == Direction.Right)
			{
				start = direction == Direction.Right ? 0 : _columnCount - 1;
				end = direction == Direction.Right ? _columnCount : -1;
			}
			else
			{
				start = direction == Direction.Down ? 0 : _rowCount - 1;
				end = direction == Direction.Down ? _rowCount : -1;
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