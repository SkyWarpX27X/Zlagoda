using DTOModels;

namespace Services.Category;

public interface ICategoryService
{
    IEnumerable<CategoryDto> GetCategories();
}