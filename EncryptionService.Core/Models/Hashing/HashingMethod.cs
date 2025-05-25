namespace EncryptionService.Core.Models.Hashing
{
	public enum HashingMethod : byte
	{
		Checksum,
		MiddleSquares,
		ModuloDivision,
		BaseConversion,
		Folding
	}
}