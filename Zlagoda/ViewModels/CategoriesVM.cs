using DTOModels;
using Services;
using System.Threading.Tasks;
using Services.Category;
using Zlagoda.Test;

namespace Zlagoda.ViewModels;

public class CategoriesVM
{
    //TODO replace with real service
    //private readonly ICategoryService _categoryService;
    public IEnumerable<CategoryDto> Categories { get; set; }
    
    public bool IsCreating { get; private set; }
    public CategoryDto? NewCategory { get; private set; }
    
    public CategoriesVM()
    {
        //_categoryService = categoryService;
        Categories = new List<CategoryDto>();
    }

    public void LoadCategories()
    {
        //Categories = _categoryService.GetCategories();
        Categories = FakeCategories.GetCategories();
    }

    public void ShowCreateNew()
    {
        NewCategory = new CategoryDto(0, "");
        IsCreating = true;
    }

    public void SaveNewCategory(CategoryDto category)
    {
        IsCreating = false;
        NewCategory = null;
        //_categoryService.AddCategory(category);
        LoadCategories();
    }

    public void CancelCreate()
    {
        IsCreating = false;
        NewCategory = null;
    }

    public void EditCategory(CategoryDto category)
    {
        //_categoryService.UpdateCategory(category);
        LoadCategories();
    }

    public void DeleteCategory(long id)
    {
        //_categoryService.DeleteCategory(id);
        LoadCategories();
    }
}
