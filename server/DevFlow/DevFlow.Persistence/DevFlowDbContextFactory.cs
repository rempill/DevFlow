using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DevFlow.Persistence;

public sealed class DevFlowDbContextFactory : IDesignTimeDbContextFactory<DevFlowDbContext>
{
    public DevFlowDbContext CreateDbContext(string[] args)
    {
        var connectionString =
            Environment.GetEnvironmentVariable("DEVFLOW_CONNECTION_STRING") ??
            "Host=localhost;Port=5432;Database=devflow;Username=postgres;Password=mihai222";

        var optionsBuilder = new DbContextOptionsBuilder<DevFlowDbContext>();
        optionsBuilder.UseNpgsql(connectionString, options =>
            options.MigrationsAssembly("DevFlow.Persistence"));

        return new DevFlowDbContext(optionsBuilder.Options);
    }
}

