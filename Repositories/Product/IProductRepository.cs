using DBModels;

namespace Repositories.Product;

public interface IProductRepository
{
    ProductDBModel? GetProduct(long id);

    IEnumerable<ProductDBModel> GetProducts(bool sortByName = true, string? categoryName = null,
        string? productName = null);
    void AddProduct(ProductDBModel product);
    void UpdateProduct(ProductDBModel product);
    void DeleteProduct(ProductDBModel product);
}