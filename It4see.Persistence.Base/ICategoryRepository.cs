using It4see.Domain;

namespace It4see.Persistence.Base;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllAsync();
    Task<Category> FindAsync(int id);
    Task<Category> FindByTitleAsync(string title);
    Task<Category> AddAsync(Category category);
    Task UpdateAsync(Category category);
    Task DeleteAsync(int id);
}