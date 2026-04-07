using DTOModels;

namespace Zlagoda.Test;

static class FakeProducts
{
    static IEnumerable<ProductDTO> _products;

    static FakeProducts()
    {
        _products = new List<ProductDTO>()
        {
            new ProductDTO(1, "Кілограм щура", "Товари подвійного призначення", "Екологи вішаються", "ПХЗ"),
            new ProductDTO(2, "Газета 'Кишинівський Підмічатель'", "Розваги",
                "Премія 'Газета року 2077'", "Rousseau&Reese")
        };
    }

    public static IEnumerable<ProductDTO> GetProducts()
    {
        return _products;
    }
}