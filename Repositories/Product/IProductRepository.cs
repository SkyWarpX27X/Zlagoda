using DBModels;

namespace Repositories.Product;

public interface IProductRepository
{
    ProductDBModel? GetProduct(long id);

    IEnumerable<ProductDBModel> GetProducts(bool sortByName = true, string? categoryName = null);
    IEnumerable<ProductDBModel> GetProductsByName(string productNameQuery);
    int GetTotalUnits(long id, (string StartDate, string EndDate) dates);
    void AddProduct(ProductDBModel product);
    void UpdateProduct(ProductDBModel product);
    void DeleteProduct(long id);
}