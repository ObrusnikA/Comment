using CommentingSystem.Web.Configurations;
using PaulMiami.AspNetCore.Mvc.Recaptcha;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViewsConfiguration();

builder.Services.AddDbContextConfiguration(builder.Configuration);

builder.Services.AddRecaptcha(new RecaptchaOptions
{
	SiteKey = builder.Configuration["Recaptcha:SiteKey"],
	SecretKey = builder.Configuration["Recaptcha:SecretKey"]
});

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.MapDefaultControllerRoute();

await app.RunAsync();