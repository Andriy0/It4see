using It4see.Domain;
using It4see.Persistence.Base;
using Microsoft.EntityFrameworkCore;

namespace It4see.Persistence.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ShopDatabaseContext dbContext;

    public CategoryRepository(ShopDatabaseContext dbContext)
    {
        this.dbContext = dbContext;
    }


    public async Task<List<Category>> GetAllAsync()
    {
        return await dbContext.Categories.ToListAsync();
    }

    public async Task<Category> FindAsync(int id)
    {
        return await dbContext.Categories.FindAsync(id);
    }

    public async Task<Category> FindByTitleAsync(string title)
    {
        return await dbContext.Categories.FirstOrDefaultAsync(c => c.Title == title);
    }

    public async Task<Category> AddAsync(Category category)
    {
        await dbContext.Categories.AddAsync(category);
        await dbContext.SaveChangesAsync();
        return category;
    }

    public async Task UpdateAsync(Category category)
    {
        var dbCategory = await dbContext.Categories.FindAsync(category.Id);
        if (dbCategory == null)
        {
            throw new NullReferenceException();
        }

        dbCategory.Title = category.Title;

        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var category = await dbContext.Categories.FindAsync(id);
        if (category == null)
        {
            throw new NullReferenceException();
        }

        dbContext.Remove(category);

        await dbContext.SaveChangesAsync();
    }
}