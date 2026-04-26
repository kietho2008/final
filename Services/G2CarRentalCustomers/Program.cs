using CarRentalPlatform.Models;
using G2_Shared_Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var serviceName = "G2Customers";
var serviceVersion = "1.0.0";
builder.Services.AddHealthChecks();
builder.Logging.ClearProviders();
builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
builder.Logging.AddConsole();
builder.Logging.AddOpenTelemetry(options =>
{
	options.SetResourceBuilder(
		ResourceBuilder.CreateDefault().AddService(serviceName: serviceName, serviceVersion: serviceVersion));
	options.IncludeFormattedMessage = true;
	options.IncludeScopes = true;
	options.ParseStateValues = true;
	options.AddConsoleExporter();
});

builder.Services.AddOpenTelemetry()
	.ConfigureResource(resource => resource.AddService(serviceName: serviceName, serviceVersion: serviceVersion))
	.WithTracing(tracing =>
	{
		tracing
			.AddAspNetCoreInstrumentation(options =>
			{
				options.RecordException = true;
				options.Filter = httpContext =>
					!httpContext.Request.Path.StartsWithSegments("/swagger") &&
					!httpContext.Request.Path.StartsWithSegments("/favicon.ico");
			})
			.AddHttpClientInstrumentation()
			.AddConsoleExporter();
	})
	.WithMetrics(metrics =>
	{
		metrics
			.AddAspNetCoreInstrumentation()
			.AddHttpClientInstrumentation()
			.AddConsoleExporter();
	});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<G2CustomerProfileContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.UseMiddleware<G2GlobalExceptionMiddleware>();
app.UseMiddleware<G2GatewayMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
// Auto create DB/table for Docker SQL Server
using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;
	var context = services.GetRequiredService<G2CustomerProfileContext>();
    
	try
	{
		var databaseCreator = context.Database.GetService<IRelationalDatabaseCreator>();
        
		// If the database doesn't exist, create it
		if (!databaseCreator.Exists()) databaseCreator.Create();
        
		// If the tables don't exist, create them
		if (!databaseCreator.HasTables()) databaseCreator.CreateTables();
        
		// Log success so you can see it in 'kubectl logs'
		Console.WriteLine("Database and Schema verified/created successfully.");
	}
	catch (Exception ex)
	{
		Console.WriteLine($"DB Initialization failed: {ex.Message}");
	}
}

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health").AllowAnonymous();

app.Run();
