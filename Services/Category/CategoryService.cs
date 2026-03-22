using DTOModels;
using Repositories.Category;

namespace Services.Category;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    
    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    
    public IEnumerable<CategoryDto> GetCategories()
    {
        return _categoryRepository.GetCategories().Select(category => new CategoryDto(category.Id, category.Name)).ToList();
    }
}