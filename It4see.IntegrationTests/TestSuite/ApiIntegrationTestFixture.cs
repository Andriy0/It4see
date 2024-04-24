using It4see.Persistence;
using It4see.IntegrationTests.TestSuite;

namespace ApiTestDemo.IntegrationTests.TestSuite;

public abstract class ApiIntegrationTestFixture
{
    private WebAppFactory _factory = null!;
    private ShopDatabaseContext _dbContext = null!;

    protected HttpClient HttpClient = null!;

    [SetUp]
    public virtual void Setup()
    {
        _factory = new WebAppFactory();
        _dbContext = _factory.CreateDbContext();
        HttpClient = HttpClientFactory.Create(_factory);
    }

    [TearDown]
    public virtual void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
        _factory.Dispose();
        HttpClient.Dispose();
    }
}
