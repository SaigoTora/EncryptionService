namespace EncryptionService.Core.Interfaces
{
	public interface IEncryptionKey<T>
	{
		T Key { get; init; }
	}
}