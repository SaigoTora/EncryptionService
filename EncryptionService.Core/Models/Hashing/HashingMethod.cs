namespace EncryptionService.Core.Models.Hashing
{
	public enum HashingMethod : byte
	{
		ChecksumParity,
		MiddleSquares,
		ModuloDivision,
		BaseConversion,
		Folding
	}
}