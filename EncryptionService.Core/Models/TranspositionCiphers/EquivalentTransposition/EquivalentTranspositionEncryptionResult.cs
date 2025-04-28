namespace EncryptionService.Core.Models.TranspositionCiphers.EquivalentTransposition
{
	public class EquivalentTranspositionEncryptionResult(string text, char[,] initialMatrix,
		char[,] transpositionMatrix, int[] transpositionIndexes)
		: EncryptionResult(text)
	{
		public char[,] InitialMatrix { get; private set; } = initialMatrix;
		public char[,] TranspositionMatrix { get; private set; } = transpositionMatrix;
		public int[] TranspositionIndexes { get; private set; } = transpositionIndexes;
	}
}