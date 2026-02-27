using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace OrderFlow.Infrastructure.Persistence;

public sealed class OrderFlowDbContextFactory : IDesignTimeDbContextFactory<OrderFlowDbContext>
{
    public OrderFlowDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<OrderFlowDbContext>();

        // Mesma connection string do appsettings.Development.json
        var connectionString =
            "Host=localhost;Port=5432;Database=orderflow;Username=orderflow;Password=orderflow";

        optionsBuilder.UseNpgsql(connectionString);

        return new OrderFlowDbContext(optionsBuilder.Options);
    }
}
