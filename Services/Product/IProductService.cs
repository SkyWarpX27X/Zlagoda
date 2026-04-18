using DTOModels;

namespace Services.Product;

public interface IProductService
{
    IEnumerable<ProductDTO> GetProducts(string? categoryName = null);
    ProductDTO? GetProduct(long id);
    int GetTotalUnits(long id, (DateTime StartDate, DateTime EndDate) dates);
    void AddProduct(ProductDTO product);
    void UpdateProduct(ProductDTO product);
    void DeleteProduct(long id);
}