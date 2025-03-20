namespace EncryptionService.Core.Models
{
	public class EquivalentTranspositionKeyData
	{
		public required int RowCount { get; init; }
		public required int ColumnCount { get; init; }
		public required int[] RowNumbers { get; init; }
		public required int[] ColumnNumbers { get; init; }
		public required Direction FirstWritingDirection { get; init; }
		public required Direction SecondWritingDirection { get; init; }
		public required Direction FirstReadingDirection { get; init; }
		public required Direction SecondReadingDirection { get; init; }
	}
}