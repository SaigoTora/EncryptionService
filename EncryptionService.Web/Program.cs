using Microsoft.AspNetCore.Mvc;

using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models;
using EncryptionService.Core.Models.StreamCiphersAndGenerators.IcgGenerator;
using EncryptionService.Core.Models.StreamCiphersAndGenerators.XorEncryption;
using EncryptionService.Core.Models.SubstitutionCiphers.HomophonicEncryption;
using EncryptionService.Core.Models.SubstitutionCiphers.PlayfairEncryption;
using EncryptionService.Core.Models.SubstitutionCiphers.SloganEncryption;
using EncryptionService.Core.Models.TranspositionCiphers.BlockTransposition;
using EncryptionService.Core.Models.TranspositionCiphers.EquivalentTransposition;
using EncryptionService.Core.Models.TranspositionCiphers.VerticalTransposition;
using EncryptionService.Core.Services.StreamCiphersAndGenerators;
using EncryptionService.Core.Services.SubstitutionCiphers;
using EncryptionService.Core.Services.TranspositionCiphers;
using EncryptionService.Web.Configurations;


var builder = WebApplication.CreateBuilder(args);
ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();
ConfigureMiddleware(app);
app.Run();


static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{// Add services to the container.
	services.AddControllersWithViews(options =>
	{
		options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
	});

	services.Configure<EncryptionSettings>(
			configuration.GetSection(nameof(EncryptionSettings)));

	services.AddScoped<IEncryptionService<EncryptionResult, BlockTranspositionKey, int[]>,
		BlockTranspositionEncryptionService>();
	services.AddScoped<IEncryptionService<VerticalTranspositionEncryptionResult,
		VerticalTranspositionKey, string>, VerticalTranspositionEncryptionService>();
	services.AddScoped<IEncryptionService<EquivalentTranspositionEncryptionResult,
		EquivalentTranspositionKey, EquivalentTranspositionKeyData>,
		EquivalentTranspositionEncryptionService>();
	services.AddScoped<IEncryptionService<SloganEncryptionResult, SloganEncryptionKey, string>,
		SloganEncryptionService>();
	services.AddScoped<IEncryptionService<PlayfairEncryptionResult, PlayfairEncryptionKey,
		string>, PlayfairEncryptionService>();
	services.AddScoped<IEncryptionService<HomophonicEncryptionResult, HomophonicEncryptionKey,
		Dictionary<char, int[]>>, HomophonicEncryptionService>();
	services.AddScoped<IEncryptionService<EncryptionResult, XorEncryptionKey, string>,
		XorEncryptionService>();
	services.AddScoped<IRandomNumbersGenerator<IcgGeneratorParameters>, IcgGeneratorService>();
}
static void ConfigureMiddleware(WebApplication app)
{// Configure the HTTP request pipeline.
	if (!app.Environment.IsDevelopment())
	{
		app.UseExceptionHandler("/Home/Error");
		app.UseHsts();
	}

	app.UseHttpsRedirection();
	app.UseRouting();

	app.UseAuthorization();

	app.MapStaticAssets();

	app.MapControllerRoute(
		name: "default",
		pattern: "{controller=Home}/{action=Index}/{id?}")
		.WithStaticAssets();
}