﻿using EncryptionService.Core.Models;

namespace EncryptionService.Configurations
{
	public class EncryptionSettings
	{
		public required BlockTranspositionKey BlockTranspositionKey { get; set; }
	}
}