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
    public IEnumerable<CategoryDTO> Categories { get; set; }
    
    public bool IsCreating { get; private set; }
    public CategoryDTO? NewCategory { get; private set; }
    
    public CategoriesVM()
    {
        //_categoryService = categoryService;
        Categories = new List<CategoryDTO>();
    }

    public void LoadCategories()
    {
        //Categories = _categoryService.GetCategories();
        Categories = FakeCategories.GetCategories();
    }

    public void ShowCreateNew()
    {
        NewCategory = new CategoryDTO(0, "");
        IsCreating = true;
    }

    public void SaveNewCategory(CategoryDTO category)
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

    public void EditCategory(CategoryDTO category)
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
