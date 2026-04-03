using DBModels;

namespace Repositories.StoreProduct;

public interface IStoreProductRepository
{
    StoreProductDBModel? GetStoreProduct(string upc);
    IEnumerable<StoreProductDBModel> GetStoreProducts(bool sortByName = true, bool sortByQuantity = false);

    public IEnumerable<StoreProductDBModel> GetStoreProductsPromotional(bool sortByName = true,
        bool sortByQuantity = false);

    IEnumerable<StoreProductDBModel> GetStoreProductsNonPromotional(bool sortByName = true, 
        bool sortByQuantity = false);
}