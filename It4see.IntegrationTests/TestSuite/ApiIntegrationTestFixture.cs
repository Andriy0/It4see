using It4see.Persistence;

namespace It4see.IntegrationTests.TestSuite;

public abstract class ApiIntegrationTestFixture
{
    private WebAppFactory _factory = null!;
    
    protected ShopDatabaseContext DbContext = null!;
    protected HttpClient HttpClient = null!;

    [SetUp]
    public virtual void Setup()
    {
        _factory = new WebAppFactory();
        DbContext = _factory.CreateDbContext();
        HttpClient = HttpClientFactory.Create(_factory);
    }

    [TearDown]
    public virtual void TearDown()
    {
        DbContext.Database.EnsureDeleted();
        DbContext.Dispose();
        _factory.Dispose();
        HttpClient.Dispose();
    }
}
