using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace NET.Backend.Blueprint.Infrastructure.DataAccess;

public class DesignTimeBlueprintDbContextFactory : IDesignTimeDbContextFactory<BlueprintDbContext>
{
    private const string DesignTimeConnectionString = "Host=localhost;Port=5432;Database=blueprint_net;Username=blueprint;Password=blueprint";

    public BlueprintDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<BlueprintDbContext>()
            .UseNpgsql(DesignTimeConnectionString)
            .Options;

        return new BlueprintDbContext(options);
    }
}
