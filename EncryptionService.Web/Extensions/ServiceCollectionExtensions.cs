using Microsoft.Extensions.Options;

using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models;
using EncryptionService.Core.Models.AsymmetricEncryption.ElGamalEncryption;
using EncryptionService.Core.Models.AsymmetricEncryption.KnapsackEncryption;
using EncryptionService.Core.Models.AsymmetricEncryption.RsaEncryption;
using EncryptionService.Core.Models.CryptoAnalysis.SubstitutionAnalyzer;
using EncryptionService.Core.Models.CryptoAnalysis.TranspositionAnalyzer;
using EncryptionService.Core.Models.StreamCiphersAndGenerators.IcgGenerator;
using EncryptionService.Core.Models.StreamCiphersAndGenerators.LfsrGenerator;
using EncryptionService.Core.Models.StreamCiphersAndGenerators.XorEncryption;
using EncryptionService.Core.Models.SubstitutionCiphers.HomophonicEncryption;
using EncryptionService.Core.Models.SubstitutionCiphers.PlayfairEncryption;
using EncryptionService.Core.Models.SubstitutionCiphers.SloganEncryption;
using EncryptionService.Core.Models.TranspositionCiphers.BlockTransposition;
using EncryptionService.Core.Models.TranspositionCiphers.EquivalentTransposition;
using EncryptionService.Core.Models.TranspositionCiphers.VerticalTransposition;
using EncryptionService.Core.Services.AsymmetricEncryption;
using EncryptionService.Core.Services.CryptoAnalysis;
using EncryptionService.Core.Services.Hashing;
using EncryptionService.Core.Services.StreamCiphersAndGenerators;
using EncryptionService.Core.Services.SubstitutionCiphers;
using EncryptionService.Core.Services.TranspositionCiphers;
using EncryptionService.Web.Configurations;

namespace EncryptionService.Web.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddEncryptionServices(this IServiceCollection services)
		{
			AddTranspositionCiphers(services);
			AddSubstitutionCiphers(services);
			AddStreamCiphers(services);
			AddAsymmetricCiphers(services);

			services.AddScoped<IRandomNumbersGenerator<IcgGeneratorParameters>,
				IcgGeneratorService>();
			services.AddScoped<IHashingService, HashingService>();

			return services;
		}
		private static void AddTranspositionCiphers(IServiceCollection services)
		{
			services.AddScoped<IEncryptionService<EncryptionResult, BlockTranspositionKey, int[]>,
				BlockTranspositionEncryptionService>();
			services.AddScoped<IEncryptionService<VerticalTranspositionEncryptionResult,
				VerticalTranspositionKey, string>, VerticalTranspositionEncryptionService>();
			services.AddScoped<IEncryptionService<EquivalentTranspositionEncryptionResult,
				EquivalentTranspositionKey, EquivalentTranspositionKeyData>,
				EquivalentTranspositionEncryptionService>();
		}
		private static void AddSubstitutionCiphers(IServiceCollection services)
		{
			services.AddScoped<IEncryptionService<SloganEncryptionResult, SloganEncryptionKey,
				string>, SloganEncryptionService>();
			services.AddScoped<IEncryptionService<PlayfairEncryptionResult, PlayfairEncryptionKey,
				string>, PlayfairEncryptionService>();
			services.AddScoped<IEncryptionService<HomophonicEncryptionResult,
				HomophonicEncryptionKey, Dictionary<char, int[]>>, HomophonicEncryptionService>();
		}
		private static void AddStreamCiphers(IServiceCollection services)
		{
			services.AddScoped<IEncryptionService<EncryptionResult, XorEncryptionKey, string>,
				XorEncryptionService>();
			services.AddScoped<IEncryptionService<LfsrEncryptionResult, LfsrEncryptionKey, int[]>,
				LfsrGeneratorService>();
		}
		private static void AddAsymmetricCiphers(IServiceCollection services)
		{
			services.AddSingleton<IEncryptionService<RsaEncryptionResult, RsaEncryptionKey,
				RsaEncryptionKeyData>, RsaEncryptionService>();
			services.AddSingleton<IEncryptionService<KnapsackEncryptionResult,
				KnapsackEncryptionKey, KnapsackEncryptionKeyData>>(provider =>
				{
					var settings = provider.GetRequiredService<IOptions<EncryptionSettings>>();
					return new KnapsackEncryptionService(settings.Value.KnapsackEncryptionKey);
				});
			services.AddSingleton<IEncryptionService<ElGamalEncryptionResult, ElGamalEncryptionKey,
				ElGamalEncryptionKeyData>>(provider =>
				{
					var settings = provider.GetRequiredService<IOptions<EncryptionSettings>>();
					return new ElGamalEncryptionService(settings.Value.ElGamalEncryptionKey);
				});
		}

		public static IServiceCollection AddSignatureServices(this IServiceCollection services)
		{
			services.AddScoped<ISignatureService<RsaEncryptionKey, RsaEncryptionKeyData>,
				RsaSignatureService>();
			services.AddScoped<ISignatureService<ElGamalEncryptionKey, ElGamalEncryptionKeyData>,
				ElGamalSignatureService>();

			return services;
		}

		public static IServiceCollection AddAnalyzerServices(this IServiceCollection services)
		{
			services.AddScoped<IEncryptionService<SubstitutionAnalyzerResult,
				SubstitutionAnalyzerKey, Dictionary<char, char>>, SubstitutionAnalyzerService>();
			services.AddScoped<IEncryptionService<TranspositionAnalyzerResult,
				TranspositionAnalyzerKey, HashSet<int>>, TranspositionAnalyzerService>();

			return services;
		}
	}
}