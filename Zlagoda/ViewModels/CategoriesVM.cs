using DTOModels;
using Services.Category;

namespace Zlagoda.ViewModels;

public class CategoriesVM
{
    private readonly ICategoryService _categoryService;
    public IEnumerable<CategoryDTO> Categories { get; set; }
    
    public bool IsCreating { get; private set; }
    public CategoryDTO? NewCategory { get; private set; }
    public string? ErrorMessage { get; private set; }
    
    public CategoriesVM(ICategoryService categoryService)
    {
        _categoryService = categoryService;
        Categories = new List<CategoryDTO>();
    }

    public void LoadCategories()
    {
        Categories = _categoryService.GetCategories();
    }

    public void ShowCreateNew()
    {
        NewCategory = new CategoryDTO(0, "");
        IsCreating = true;
        ErrorMessage = null;
    }

    public void SaveNewCategory(CategoryDTO category)
    {
        try
        {
            _categoryService.AddCategory(category);
            IsCreating = false;
            NewCategory = null;
            ErrorMessage = null;
        }
        catch (InvalidDataException e)
        {
            ErrorMessage = e.Message;
        }
        
        LoadCategories();
    }

    public void CancelCreate()
    {
        IsCreating = false;
        NewCategory = null;
        ErrorMessage = null;
    }

    public void ClearError()
    {
        ErrorMessage = null;
    }

    public void EditCategory(CategoryDTO category)
    {
        try
        {
            _categoryService.UpdateCategory(category);
        }
        catch (InvalidDataException e)
        {
            ErrorMessage = e.Message;
        }
        LoadCategories();
    }

    public void DeleteCategory(long id)
    {
        _categoryService.DeleteCategory(id);
        LoadCategories();
    }
}
