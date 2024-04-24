using It4see.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace It4see.IntegrationTests.TestSuite
{
    public class ShopDatabaseContextFactory
    {
        public static ShopDatabaseContext CreateWithSqlLite()
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "sql_lite_tests" };
            var connectionString = connectionStringBuilder.ToString();

            var options = new DbContextOptionsBuilder<ShopDatabaseContext>()
                .UseSqlite(connectionString)
                .Options;

            var applicationDbContext = new ShopDatabaseContext(options);
            applicationDbContext.Database.EnsureCreated();

            return applicationDbContext;
        }
    }
}
