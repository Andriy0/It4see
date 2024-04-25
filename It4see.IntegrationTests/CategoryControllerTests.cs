using It4see.Domain;
using It4see.IntegrationTests.TestSuite;
using System.Net;
using System.Net.Http.Json;
using It4see.Web.ViewModels.Category;

namespace It4see.IntegrationTests
{
    [TestFixture]
    public class CategoryControllerTests : ApiIntegrationTestFixture
    {
        [Test]
        public async Task Post_ValidCategory_ReturnsCategory()
        {
            var categoryDto = new CreateCategoryViewModel { Title = "Title" };

            var httpResponseMessage = await HttpClient.PostAsJsonAsync("Category", categoryDto);
            var categoryFromResponse = await httpResponseMessage.DeserializeAsync<CategoryDetailsViewModel>();

            Assert.Multiple(() =>
            {
                Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(categoryFromResponse!.Title, Is.EqualTo("Title"));
            });
        }

        [Test]
        public async Task Get_ExistingCategory_ReturnsCategory()
        {
            var category = new Category { Title = "Title" };
            await DbContext.Categories.AddAsync(category);
            await DbContext.SaveChangesAsync();

            var httpResponseMessage = await HttpClient.GetAsync($"Category?id={category.Id}");
            var categoryDtoFromResponse = await httpResponseMessage.DeserializeAsync<CategoryDetailsViewModel>();
            
            Assert.Multiple(() =>
            {
                Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(categoryDtoFromResponse!.Title, Is.EqualTo("Title"));
            });
        }

        [Test]
        public async Task Get_NonExistingCategory_Returns404NotFound()
        {
            var httpResponseMessage = await HttpClient.GetAsync("Category?id=1");

            Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
        
        [Test]
        public async Task Get_ExistingCategory_ByTitle_ReturnsCategory()
        {
            var category = new Category { Title = "Title" };
            await DbContext.Categories.AddAsync(category);
            await DbContext.SaveChangesAsync();

            var httpResponseMessage = await HttpClient.GetAsync($"Category/ByTitle?title={category.Title}");
            var categoryDtoFromResponse = await httpResponseMessage.DeserializeAsync<CategoryDetailsViewModel>();
            
            Assert.Multiple(() =>
            {
                Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(categoryDtoFromResponse!.Title, Is.EqualTo("Title"));
            });
        }
        
        [Test]
        public async Task Get_NonExistingCategory_ByTitle_Returns404NotFound()
        {
            var httpResponseMessage = await HttpClient.GetAsync("Category/ByTitle?title='Title'");

            Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task Get_CategoryList_ReturnsEmptyList_WhenNoCategories()
        {
            var httpResponseMessage = await HttpClient.GetAsync("Category/list");
            var categoriesDtoFromResponse = await httpResponseMessage.DeserializeAsync<List<CategoryListViewModel>>();

            Assert.Multiple(() =>
            {
                Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(categoriesDtoFromResponse, Is.Not.Null);
                Assert.That(categoriesDtoFromResponse, Is.Empty);
            });
        }

        [Test]
        public async Task Get_CategoryList_ReturnsListOfCategories_WhenCategoriesPresent()
        {
            var categories = new List<Category>
            {
                new() { Title = "Title 1" },
                new() { Title = "Title 2" },
                new() { Title = "Title 3" }
            };
            
            await DbContext.Categories.AddRangeAsync(categories);
            await DbContext.SaveChangesAsync();

            var httpResponseMessage = await HttpClient.GetAsync("Category/list");
            var categoriesDtoFromResponse = await httpResponseMessage.DeserializeAsync<List<CategoryListViewModel>>();

            Assert.Multiple(() =>
            {
                Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(categoriesDtoFromResponse, Is.Not.Empty);
                Assert.That(categoriesDtoFromResponse!.ConvertAll((c) => c.Title), Is.EqualTo(new List<string> {"Title 1", "Title 2", "Title 3"}));
            });
        }
        
        [Test]
        public async Task Put_NonExistingCategory_ReturnsInternalServerError()
        {
            var modifiedCategory = new UpdateCategoryViewModel { Id = 1, Title = "Modified title" };
        
            var httpResponseMessage = await HttpClient.PutAsJsonAsync("Category", modifiedCategory);
            
            Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
        }
        
        [Test]
        public async Task Put_ExistingCategory_ReturnsModifiedCategory()
        {
            var category = new Category { Title = "Title" };
            await DbContext.Categories.AddAsync(category);
            await DbContext.SaveChangesAsync();

            var modifiedCategory = new Category { Id = category.Id, Title = "Modified title" };

            var httpResponseMessage = await HttpClient.PutAsJsonAsync("Category", modifiedCategory);
            var categoryDtoFromResponse = await httpResponseMessage.DeserializeAsync<CategoryDetailsViewModel>();
            
            Assert.Multiple(() =>
            {
                Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(categoryDtoFromResponse!.Title, Is.EqualTo("Modified title"));
            });
        }
        
        [Test]
        public async Task Delete_NonExistingCategory_ReturnsInternalServerError()
        {
            var httpResponseMessage = await HttpClient.DeleteAsync("Category?id=1");
            
            Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
        }
        
        [Test]
        public async Task Delete_ExistingCategory_ReturnsNoContent()
        {
            var category = new Category { Title = "Title" };
            await DbContext.Categories.AddAsync(category);
            await DbContext.SaveChangesAsync();
            
            var httpResponseMessage = await HttpClient.DeleteAsync($"Category?id={category.Id}");
            
            Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }
    }
}
