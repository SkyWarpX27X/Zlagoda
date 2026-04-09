using DTOModels;

namespace Services.Category;

public interface ICategoryService
{
    IEnumerable<CategoryDTO> GetCategories();
}