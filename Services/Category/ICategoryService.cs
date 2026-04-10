using DTOModels;

namespace Services.Category;

public interface ICategoryService
{
    IEnumerable<CategoryDTO> GetCategories();
    CategoryDTO? GetCategory(long id);
    void AddCategory(CategoryDTO category);
    void UpdateCategory(CategoryDTO category);
    void DeleteCategory(long id);
}