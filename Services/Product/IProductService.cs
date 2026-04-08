using DTOModels;

namespace Services.Product;

public interface IProductService
{
    IEnumerable<ProductDTO> GetProducts();
    ProductDTO GetProduct(long id);
    void AddProduct(ProductDTO customer);
    void UpdateProduct(ProductDTO customer);
    void DeleteProduct(long id);
}