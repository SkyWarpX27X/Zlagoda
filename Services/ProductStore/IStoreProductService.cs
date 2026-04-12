using DTOModels;

namespace Services.ProductStore;

public interface IStoreProductService
{
    public IEnumerable<StoreProductDTO> GetStoreProducts(bool sortByQuantity = false);
    public StoreProductDTO GetStoreProduct(string upc);
    void AddStoreProduct(StoreProductModifyDTO storeProduct);
    void UpdateStoreProduct(StoreProductModifyDTO storeProduct);
    void DeleteStoreProduct(string upc);
}