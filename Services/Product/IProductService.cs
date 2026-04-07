using DTOModels;

namespace Services.Product;

public interface IProductService
{
    IEnumerable<ProductDTO> GetProducts();
    ProductDTO GetProduct(int id);
    void AddProduct(ProductDTO customer);
    void UpdateProduct(ProductDTO customer);
    void DeleteProduct(int id);
}