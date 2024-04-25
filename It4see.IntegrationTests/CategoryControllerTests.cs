using It4see.Domain;
using It4see.IntegrationTests.TestSuite;
using System.Net;
using System.Net.Http.Json;

namespace It4see.IntegrationTests
{
    [TestFixture]
    public class CategoryControllerTests : ApiIntegrationTestFixture
    {
        [Test]
        public async Task Post_ValidCategory_ReturnsCategory()
        {
            var todo = new Category { Title = "Title" };

            var httpResponseMessage = await HttpClient.PostAsJsonAsync("Category", todo);
            var categoryFromResponse = await httpResponseMessage.DeserializeAsync<Category>();

            Assert.Multiple(() =>
            {
                Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(categoryFromResponse!.Id, Is.EqualTo(1));
                Assert.That(categoryFromResponse!.Title, Is.EqualTo("Title"));
            });
        }

        [Test]
        public async Task Get_ExistingCategory_ReturnsCategory()
        {
            var category = new Category { Title = "Title" };
            await HttpClient.PostAsJsonAsync("Category", category);

            var httpResponseMessage = await HttpClient.GetAsync("Category?id=1");
            var categoryFromResponse = await httpResponseMessage.DeserializeAsync<Category>();
            
            Assert.Multiple(() =>
            {
                Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(categoryFromResponse!.Id, Is.EqualTo(1));
                Assert.That(categoryFromResponse!.Title, Is.EqualTo("Title"));
            });
        }

        [Test]
        public async Task Get_NonExistingCategory_Returns404NotFound()
        {
            var httpResponseMessage = await HttpClient.GetAsync("Category?id=1");

            Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task Get_CategoryList_ReturnsEmptyList_WhenNoCategories()
        {
            var httpResponseMessage = await HttpClient.GetAsync("Category/list");
            var categoriesFromResponse = await httpResponseMessage.DeserializeAsync<List<Category>>();

            Assert.Multiple(() =>
            {
                Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(categoriesFromResponse, Is.Not.Null);
                Assert.That(categoriesFromResponse, Is.Empty);
            });
        }

        [Test]
        public async Task Get_CategoryList_ReturnsListOfCategories_WhenCategoriesPresent()
        {
            // await HttpClient.PostAsJsonAsync("Category", new Category { Title = "Title 1" });
            // await HttpClient.PostAsJsonAsync("Category", new Category { Title = "Title 2" });
            // await HttpClient.PostAsJsonAsync("Category", new Category { Title = "Title 3" });
            // await dbContext.Categories.AddAsync(new Category { Title = "Title 1" });
            // await dbContext.Categories.AddAsync(new Category { Title = "Title 2" });
            // await dbContext.Categories.AddAsync(new Category { Title = "Title 3" });
            // await dbContext.SaveChangesAsync();
            var categories = new List<Category>
            {
                new() { Title = "Title 1" },
                new() { Title = "Title 2" },
                new() { Title = "Title 3" }
            };
            
            await DbContext.Categories.AddRangeAsync(categories);
            await DbContext.SaveChangesAsync();

            var httpResponseMessage = await HttpClient.GetAsync("Category/list");
            var categoriesFromResponse = await httpResponseMessage.DeserializeAsync<List<Category>>();

            Assert.Multiple(() =>
            {
                Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(categoriesFromResponse, Is.Not.Empty);
                Assert.That(categoriesFromResponse!.ConvertAll((c) => c.Title), Is.EqualTo(new List<string> {"Title 1", "Title 2", "Title 3"}));
                // Assert.That(categoriesFromResponse, Is.EqualTo(categories));
            });
        }
    }
}
