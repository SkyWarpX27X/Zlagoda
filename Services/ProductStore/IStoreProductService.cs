using DTOModels;

namespace Services.ProductStore;

public interface IStoreProductService
{
    public IEnumerable<StoreProductDTO> GetStoreProducts(bool sortByQuantity = false);
    public StoreProductDTO GetStoreProduct(string upc);
    void AddStoreProduct(StoreProductModifyDTO storeProduct);
    void UpdateStoreProduct(StoreProductModifyDTO storeProduct);
    void AddPromotionalStoreProduct(string originalUpc, string promotionalUpc);
    void DeletePromotionalStoreProduct(string promotionalUpc);
    void DeleteStoreProduct(string upc);
}