using DTOModels;

namespace Zlagoda.Test;

static class FakeStoreProducts
{
    private static IEnumerable<StoreProductDTO> _products;

    static FakeStoreProducts()
    {
        _products = new List<StoreProductDTO>()
        {
            new StoreProductDTO("12345678", "Газета Кишинівський Підмічатель", 30, 100, false, null),
            new StoreProductDTO("3123132", "Повітря", 20, 100, true, 30)
        };
    }
    
    public static IEnumerable<StoreProductDTO> GetProducts()
    {
        return _products;
    }
}