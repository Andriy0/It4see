using It4see.Persistence;
using It4see.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace It4see.IntegrationTests.TestSuite;

public class WebAppFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(ReplaceDbContextWithInMemoryDb);
    }
    
    private static void ReplaceDbContextWithInMemoryDb(IServiceCollection services)
    {
        var existingDbContextRegistration = services.SingleOrDefault(
            d => d.ServiceType == typeof(DbContextOptions<ShopDatabaseContext>)
        );

        if (existingDbContextRegistration != null)
        {
            services.Remove(existingDbContextRegistration);
        }

        var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "sql_lite_tests" };
        var connectionString = connectionStringBuilder.ToString();
        services.AddDbContext<ShopDatabaseContext>(options =>
            options.UseSqlite(connectionString));
    }

    public ShopDatabaseContext CreateDbContext()
    {
        var dbContext = Services.CreateScope().ServiceProvider.GetService<ShopDatabaseContext>()!;
        dbContext.Database.EnsureCreated();

        return dbContext;
    }
}
