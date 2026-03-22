using DTOModels;
using Services;
using System.Threading.Tasks;
using Services.Category;

namespace Zlagoda.ViewModels;

public class CategoriesVM
{
    private readonly ICategoryService _categoryService;
    public IEnumerable<CategoryDto> Categories { get; set; }
    
    public CategoriesVM(ICategoryService categoryService)
    {
        _categoryService = categoryService;
        Categories = new List<CategoryDto>();
    }

    public void LoadCategories()
    {
        Categories = _categoryService.GetCategories();
    }
}