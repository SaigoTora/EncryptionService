using Microsoft.AspNetCore.Mvc;

using EncryptionService.Web.Configurations;
using EncryptionService.Web.Extensions;


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

	services.AddEncryptionServices()
			.AddSignatureServices()
			.AddAnalyzerServices();
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