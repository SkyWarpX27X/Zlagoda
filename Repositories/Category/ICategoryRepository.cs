using DBModels;

namespace Repositories.Category;

public interface ICategoryRepository
{
    CategoryDBModel? GetCategory(long id);
    CategoryDBModel? GetCategory(string categoryName);
    IEnumerable<CategoryDBModel> GetCategories(bool sortByName = true);
    void AddCategory(CategoryDBModel category);
    void UpdateCategory(CategoryDBModel category);
    void DeleteCategory(long id);
}