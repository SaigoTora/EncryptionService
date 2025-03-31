using Microsoft.AspNetCore.Mvc;

using EncryptionService.Configurations;
using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models;
using EncryptionService.Core.Models.BlockTransposition;
using EncryptionService.Core.Models.EquivalentTransposition;
using EncryptionService.Core.Models.SloganEncryption;
using EncryptionService.Core.Models.VerticalTransposition;
using EncryptionService.Core.Services;
using EncryptionService.Core.Models.PlayfairEncryption;
using EncryptionService.Core.Models.HomophonicEncryption;


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
builder.Services.AddScoped<IEncryptionService<EncryptionResult, HomophonicEncryptionKey,
	Dictionary<char, int[]>>, HomophonicEncryptionService>();


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