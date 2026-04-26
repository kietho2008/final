using CarRentalPlatform.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHealthChecks();

var gatewayUrl = builder.Configuration["ApiGateway:BaseUrl"]!;
const string gatewayKey = "GS-Secret-Key-2111";

builder.Services.AddHttpClient("ApiGateway", client =>
{
	client.BaseAddress = new Uri(gatewayUrl);
	client.DefaultRequestHeaders.Add("X-GS-Api-Key", gatewayKey);
});

builder.Services.AddControllersWithViews();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapHealthChecks("/health").AllowAnonymous();
app.Run();