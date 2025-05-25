using EncryptionService.Core.Models.Hashing;

namespace EncryptionService.Core.Interfaces
{
	public interface IHashingService
	{
		string ComputeHash(string text, HashingMethod method);
		bool VerifyHash(string text, string hash, HashingMethod method);
	}
}