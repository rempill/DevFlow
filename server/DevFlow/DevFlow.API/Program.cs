using DevFlow.Application.DependencyInjection;
using DevFlow.Infrastructure.DependencyInjection;
using DevFlow.Persistence.DependencyInjection;
using DevFlow.Persistence.Seeding;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DevFlow");
if (string.IsNullOrWhiteSpace(connectionString))
{
	throw new InvalidOperationException("Connection string 'DevFlow' is not configured.");
}

builder.Services.AddPersistence(connectionString);
builder.Services.AddApplicationServices();
builder.Services.AddGitHubAdapter(
	builder.Configuration["GitHub:ProductName"] ?? "DevFlow",
	Environment.GetEnvironmentVariable("GITHUB_TOKEN"));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DevFlow.Persistence.DevFlowDbContext>();
    await DevFlowSeeder.SeedAsync(dbContext);
}

app.MapControllers();

app.Run();
