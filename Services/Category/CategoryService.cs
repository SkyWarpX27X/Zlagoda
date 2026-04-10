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
    
    public IEnumerable<CategoryDTO> GetCategories()
    {
        foreach (var category in _categoryRepository.GetCategories())
        {
            yield return new(category.Id, category.Name);
        }
    }

    public CategoryDTO? GetCategory(long id)
    {
        var category = _categoryRepository.GetCategory(id);
        if (category is null) return null;
        return new(category.Id, category.Name);
    }

    public void AddCategory(CategoryDTO category)
    {
        if (string.IsNullOrEmpty(category.Name)) throw new InvalidDataException("Name is required");
        _categoryRepository.AddCategory(new(category.Id, category.Name));
    }

    public void UpdateCategory(CategoryDTO category)
    {
        if (string.IsNullOrEmpty(category.Name)) throw new InvalidDataException("Name is required");
        _categoryRepository.UpdateCategory(new(category.Id, category.Name));
    }

    public void DeleteCategory(long id)
    {
        _categoryRepository.DeleteCategory(id);
    }
}