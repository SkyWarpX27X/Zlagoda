using DTOModels;

namespace Zlagoda.Test;

static class FakeCategories
{
    static IEnumerable<CategoryDto> _categories;

    static FakeCategories()
    {
        _categories = new List<CategoryDto>()
        {
            new CategoryDto(1, "Картон"),
            new CategoryDto(2, "Товари подвійного призначення")
        };
    }

    public static IEnumerable<CategoryDto> GetCategories()
    {
        return _categories;
    }
}