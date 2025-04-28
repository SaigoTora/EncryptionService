using Microsoft.AspNetCore.Mvc;

using EncryptionService.Configurations;
using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models;
using EncryptionService.Core.Models.TranspositionCiphers.EquivalentTransposition;
using EncryptionService.Core.Models.TranspositionCiphers.VerticalTransposition;
using EncryptionService.Core.Models.TranspositionCiphers.BlockTransposition;
using EncryptionService.Core.Models.SubstitutionCiphers.HomophonicEncryption;
using EncryptionService.Core.Models.SubstitutionCiphers.PlayfairEncryption;
using EncryptionService.Core.Models.SubstitutionCiphers.SloganEncryption;
using EncryptionService.Core.Models.StreamCiphersAndGenerators.XorEncryption;
using EncryptionService.Core.Models.StreamCiphersAndGenerators.IcgGenerator;
using EncryptionService.Core.Services.TranspositionCiphers;
using EncryptionService.Core.Services.SubstitutionCiphers;
using EncryptionService.Core.Services.StreamCiphersAndGenerators;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
	options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
});
builder.Services.Configure<EncryptionSettings>(
		builder.Configuration.GetSection(nameof(EncryptionSettings)));
builder.Services.AddScoped<IEncryptionService<EncryptionResult, BlockTranspositionKey, int[]>,
	BlockTranspositionEncryptionService>();
builder.Services.AddScoped<IEncryptionService<VerticalTranspositionEncryptionResult,
	VerticalTranspositionKey, string>, VerticalTranspositionEncryptionService>();
builder.Services.AddScoped<IEncryptionService<EquivalentTranspositionEncryptionResult,
	EquivalentTranspositionKey, EquivalentTranspositionKeyData>,
	EquivalentTranspositionEncryptionService>();
builder.Services.AddScoped<IEncryptionService<SloganEncryptionResult, SloganEncryptionKey, string>,
	SloganEncryptionService>();
builder.Services.AddScoped<IEncryptionService<PlayfairEncryptionResult, PlayfairEncryptionKey,
	string>, PlayfairEncryptionService>();
builder.Services.AddScoped<IEncryptionService<HomophonicEncryptionResult, HomophonicEncryptionKey,
	Dictionary<char, int[]>>, HomophonicEncryptionService>();
builder.Services.AddScoped<IEncryptionService<EncryptionResult, XorEncryptionKey, string>,
	XorEncryptionService>();
builder.Services.AddScoped<IRandomNumbersGenerator<IcgGeneratorParameters>, IcgGeneratorService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
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


app.Run();