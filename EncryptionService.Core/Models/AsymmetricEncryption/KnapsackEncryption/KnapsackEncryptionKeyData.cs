namespace EncryptionService.Core.Models.AsymmetricEncryption.KnapsackEncryption
{
	public class KnapsackEncryptionKeyData
	{
		public required int D0 { get; init; }
		public required int StepMin { get; init; }
		public required int StepMax { get; init; }
	}
}