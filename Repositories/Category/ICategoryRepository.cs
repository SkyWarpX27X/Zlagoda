using DBModels;

namespace Repositories.Category;

public interface ICategoryRepository
{
    IEnumerable<CategoryDBModel> GetCategories();
}