﻿namespace EncryptionService.Core.Models.SloganEncryption
{
	public class SloganEncryptionResult(string text, Dictionary<char, char> encryptionTable)
		: EncryptionResult(text)
	{
		public Dictionary<char, char> EncryptionTable { get; private set; } = encryptionTable;
	}
}