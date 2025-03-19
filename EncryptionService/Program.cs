using Microsoft.AspNetCore.Mvc;

using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models;
using EncryptionService.Core.Services;
using EncryptionService.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
	options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
});
builder.Services.Configure<EncryptionSettings>(
		builder.Configuration.GetSection(nameof(EncryptionSettings)));
builder.Services.AddScoped<IEncryptionService<BlockTranspositionKey, int[]>,
	BlockTranspositionEncryptionService>();
builder.Services.AddScoped<IEncryptionService<VerticalTranspositionKey, string>,
	VerticalTranspositionEncryptionService>();


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