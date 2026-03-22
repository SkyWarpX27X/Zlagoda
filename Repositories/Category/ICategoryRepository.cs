using DBModels;

namespace Repositories.Category;

public interface ICategoryRepository
{
    IEnumerable<CategoryDb> GetCategories();
}