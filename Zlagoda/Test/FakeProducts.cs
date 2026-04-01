using DTOModels;

namespace Zlagoda.Test;

static class FakeProducts
{
    static IEnumerable<ProductDto> _products;

    static FakeProducts()
    {
        _products = new List<ProductDto>()
        {
            new ProductDto(1, "Кілограм щура", "Товари подвійного призначення", "Екологи вішаються", "ПХЗ"),
            new ProductDto(2, "Газета 'Кишинівський Підмічатель'", "Розваги",
                "Премія 'Газета року 2077'", "Rousseau&Reese")
        };
    }

    public static IEnumerable<ProductDto> GetProducts()
    {
        return _products;
    }
}