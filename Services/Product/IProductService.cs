using DTOModels;

namespace Services.Product;

public interface IProductService
{
    IEnumerable<ProductDTO> GetProducts(string? categoryName = null);
    ProductDTO? GetProduct(long id);
    void AddProduct(ProductDTO product);
    void UpdateProduct(ProductDTO product);
    void DeleteProduct(long id);
}