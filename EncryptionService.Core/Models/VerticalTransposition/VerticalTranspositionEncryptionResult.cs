namespace EncryptionService.Core.Models.VerticalTransposition
{
	public class VerticalTranspositionEncryptionResult(string text, char[,] matrix, int[] indexes)
		: EncryptionResult(text)
	{
		public char[,] Matrix { get; private set; } = matrix;
		public int[] Indexes { get; private set; } = indexes;
	}
}