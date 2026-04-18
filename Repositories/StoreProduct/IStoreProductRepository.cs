using DBModels;

namespace Repositories.StoreProduct;

public interface IStoreProductRepository
{
    StoreProductInfoDataModel? GetStoreProduct(string upc);
    IEnumerable<StoreProductInfoDataModel> GetStoreProducts(bool sortByName = true, bool sortByQuantity = false);

    public IEnumerable<StoreProductInfoDataModel> GetStoreProductsPromotional(bool sortByName = true,
        bool sortByQuantity = false);

    IEnumerable<StoreProductInfoDataModel> GetStoreProductsNonPromotional(bool sortByName = true, 
        bool sortByQuantity = false);

    StoreProductInfoDataModel? GetNonPromByProm(string upcProm);
    (StoreProductInfoDataModel? nonProm, StoreProductInfoDataModel? prom) GetStoreProductsByProductId(long productId);
    void AddStoreProduct(StoreProductDBModel storeProduct);
    void UpdateStoreProduct(StoreProductDBModel storeProduct);
    void DeleteStoreProduct(string upc);
}