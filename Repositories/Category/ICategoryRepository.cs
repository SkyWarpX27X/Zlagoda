using DBModels;

namespace Repositories.Category;

public interface ICategoryRepository
{
    CategoryDBModel? GetCategory(long id);
    IEnumerable<CategoryDBModel> GetCategories(bool sortByName = true);
}