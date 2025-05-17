using EncryptionService.Core.Interfaces;

namespace EncryptionService.Core.Models.StreamCiphersAndGenerators.LfsrGenerator
{
	public class LfsrGeneratorKey : IEncryptionKey<int[]>
	{
		public required int[] Key { get; init; }
		public bool[] InitialState { get; set; } = [];

		public void SetInitialState(string state)
		{
			int length = Key?.Max() ?? 0;
			InitialState = new bool[length];

			for (int i = 0; i < length && i < state.Length; i++)
				if (state[i] == '1')
					InitialState[i] = true;
		}
	}
}