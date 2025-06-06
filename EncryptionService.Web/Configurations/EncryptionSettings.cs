﻿using EncryptionService.Core.Models.AsymmetricEncryption.ElGamalEncryption;
using EncryptionService.Core.Models.AsymmetricEncryption.KnapsackEncryption;
using EncryptionService.Core.Models.AsymmetricEncryption.RsaEncryption;
using EncryptionService.Core.Models.StreamCiphersAndGenerators.IcgGenerator;
using EncryptionService.Core.Models.StreamCiphersAndGenerators.LfsrGenerator;
using EncryptionService.Core.Models.StreamCiphersAndGenerators.XorEncryption;
using EncryptionService.Core.Models.SubstitutionCiphers.PlayfairEncryption;
using EncryptionService.Core.Models.SubstitutionCiphers.SloganEncryption;
using EncryptionService.Core.Models.TranspositionCiphers.BlockTransposition;
using EncryptionService.Core.Models.TranspositionCiphers.EquivalentTransposition;
using EncryptionService.Core.Models.TranspositionCiphers.VerticalTransposition;

namespace EncryptionService.Web.Configurations
{
	public class EncryptionSettings
	{
		public required BlockTranspositionKey BlockTranspositionKey { get; set; }
		public required VerticalTranspositionKey VerticalTranspositionKey { get; set; }
		public required EquivalentTranspositionKey EquivalentTranspositionKey { get; set; }
		public required SloganEncryptionKey SloganEncryptionKey { get; set; }
		public required PlayfairEncryptionKey PlayfairEncryptionKey { get; set; }
		public required Dictionary<string, int> HomophonicEncryptionFrequency { get; set; }
		public required XorEncryptionKey XorEncryptionKey { get; set; }
		public required IcgGeneratorParameters IcgGeneratorParameters { get; set; }
		public required LfsrEncryptionKey LfsrEncryptionKey { get; set; }
		public required RsaEncryptionKey RsaEncryptionKey { get; set; }
		public required KnapsackEncryptionKey KnapsackEncryptionKey { get; set; }
		public required ElGamalEncryptionKey ElGamalEncryptionKey { get; set; }
	}
}