using DTOModels;

namespace Zlagoda.Test;

static class FakeCategories
{
    static IEnumerable<CategoryDTO> _categories;

    static FakeCategories()
    {
        _categories = new List<CategoryDTO>()
        {
            new CategoryDTO(1, "Картон"),
            new CategoryDTO(2, "Товари подвійного призначення")
        };
    }

    public static IEnumerable<CategoryDTO> GetCategories()
    {
        return _categories;
    }
}