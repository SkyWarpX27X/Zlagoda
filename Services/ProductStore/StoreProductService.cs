using DBModels;
using DTOModels;
using Repositories.Product;
using Repositories.StoreProduct;

namespace Services.ProductStore;

public class StoreProductService : IStoreProductService
{
    private readonly IStoreProductRepository _storeProductRepository;
    private readonly IProductRepository _productRepository;

    public StoreProductService(IStoreProductRepository storeProductRepository, IProductRepository productRepository)
    {
        _storeProductRepository = storeProductRepository;
        _productRepository = productRepository;
    }

    public IEnumerable<StoreProductDTO> GetStoreProducts(bool sortByQuantity, bool sortByName)
    {
        foreach (var storeProduct in _storeProductRepository.GetStoreProductsNonPromotional(sortByName, sortByQuantity))
        {
            if (storeProduct.UPCProm is null)
            {
                yield return StoreProductDbToDto(storeProduct);
                continue;
            }
            
            var promotional = _storeProductRepository.GetStoreProduct(storeProduct.UPCProm)
                              ?? throw new InvalidDataException("Promotional store product does not exist.");
            if (promotional.Quantity > 0)
            {
                yield return StoreProductDbToDto(promotional);
                continue;
            }
            
            yield return StoreProductDbToDto(storeProduct);
        }
    }

    public IEnumerable<StoreProductDTO> GetNonPromotionalStoreProducts(bool sortByQuantity, bool sortByName)
    {
        foreach (var storeProduct in _storeProductRepository.GetStoreProductsNonPromotional(sortByName, sortByQuantity))
        {
            if (storeProduct.UPCProm is null)
            {
                yield return StoreProductDbToDto(storeProduct);
                continue;
            }
            
            var promotional = _storeProductRepository.GetStoreProduct(storeProduct.UPCProm)
                              ?? throw new InvalidDataException("Promotional store product does not exist.");
            if (promotional.Quantity == 0)
                yield return StoreProductDbToDto(storeProduct);
        }
    }

    public IEnumerable<StoreProductDTO> GetPromotionalStoreProducts(bool sortByQuantity, bool sortByName)
    {
        foreach (var storeProduct in _storeProductRepository.GetStoreProductsPromotional(sortByName, sortByQuantity))
            if (storeProduct.Quantity > 0)
                yield return StoreProductDbToDto(storeProduct);
    }

    public StoreProductDTO GetStoreProduct(string upc)
    {
        var storeProduct = _storeProductRepository.GetStoreProduct(upc);
        if (storeProduct is null) throw new InvalidDataException("Product does not exist");
        return StoreProductDbToDto(storeProduct);
    }

    public void AddStoreProduct(StoreProductModifyDTO storeProduct)
    {
        Validation.ValidateStoreProductCreate(storeProduct);

        StoreProductDBModel result = new(
            storeProduct.Upc, 
            null,
            storeProduct.ProductId,
            storeProduct.Price,
            storeProduct.Quantity, 
            false);
        
        var existing = _storeProductRepository.GetStoreProduct(storeProduct.Upc);
        if (existing is not null)
        {
            result.UPCProm = existing.UPCProm;
            result.Quantity += existing.Quantity;
            _storeProductRepository.UpdateStoreProduct(result);

            if (existing.UPCProm is null) return;
            var promo = _storeProductRepository.GetStoreProduct(existing.UPCProm)
                        ?? throw new InvalidDataException("Store product does not exist.");
            result.UPC = promo.UPC;
            result.UPCProm = null;
            result.Quantity += promo.Quantity;
            result.SellingPrice *= 0.8m;
            _storeProductRepository.UpdateStoreProduct(result);
            
            return;
        }
        
        _storeProductRepository.AddStoreProduct(result);
    }

    public void UpdateStoreProduct(StoreProductModifyDTO storeProduct)
    {
        Validation.ValidateStoreProductUpdate(storeProduct);
        
        var existing = _storeProductRepository.GetStoreProduct(storeProduct.Upc)
            ?? throw new InvalidDataException("Store product does not exist.");
        
        StoreProductDBModel result = new(
            storeProduct.Upc, 
            existing.UPCProm,
            storeProduct.ProductId,
            storeProduct.Price,
            storeProduct.Quantity, 
            false);
        
        _storeProductRepository.UpdateStoreProduct(result);
    }

    public void AddPromotionalStoreProduct(string originalUpc, string promotionalUpc)
    {
        Validation.ValidateStoreProductMakePromotional(promotionalUpc);
        
        var storeProduct = _storeProductRepository.GetStoreProduct(originalUpc)
                       ?? throw new InvalidDataException("Store product does not exist.");

        StoreProductDBModel result = new(
            promotionalUpc, 
            null,
            storeProduct.ProductId,
            storeProduct.SellingPrice * 0.8m,
            storeProduct.Quantity, 
            true);
        
        var promotional = _storeProductRepository.GetStoreProduct(promotionalUpc);
        if (promotional is not null) _storeProductRepository.UpdateStoreProduct(result);
        else _storeProductRepository.AddStoreProduct(result);
        
        result.UPC = originalUpc;
        result.UPCProm = promotionalUpc;
        result.Quantity = 0;
        _storeProductRepository.UpdateStoreProduct(result);
    }

    public void DeletePromotionalStoreProduct(string promotionalUpc)
    {
        var promotional = _storeProductRepository.GetStoreProduct(promotionalUpc)
                          ?? throw new InvalidDataException("Promotional store product doesn't exist.");
        var original = _storeProductRepository.GetNonPromByProm(promotionalUpc)
                       ?? throw new InvalidDataException("Store product doesn't exist.");
        
        StoreProductDBModel result = new(
            original.UPC, 
            original.UPCProm,
            original.ProductId,
            original.SellingPrice,
            promotional.Quantity, 
            false);
        _storeProductRepository.UpdateStoreProduct(result);
        
        result = new(
            promotional.UPC, 
            null,
            promotional.ProductId,
            promotional.SellingPrice,
            0, 
            true);
        _storeProductRepository.UpdateStoreProduct(result);
    }

    public void DeleteStoreProduct(string upc)
    {
        if (_storeProductRepository.IsInReceipt(upc))
            throw new InvalidOperationException("Can't delete a store product which is already in a receipt.");
        
        var storeProduct = _storeProductRepository.GetStoreProduct(upc)
                           ?? throw new InvalidDataException("Store product doesn't exist.");
        if (storeProduct.UPCProm is not null)
        {
            if (_storeProductRepository.IsInReceipt(storeProduct.UPCProm))
                throw new InvalidOperationException("Can't delete a store product which is already in receipt.");
            _storeProductRepository.DeleteStoreProduct(storeProduct.UPCProm);
        }
            
        _storeProductRepository.DeleteStoreProduct(storeProduct.UPC);
    }

    private StoreProductDTO StoreProductDbToDto(StoreProductInfoDataModel storeProduct)
    {
        var product = _productRepository.GetProduct(storeProduct.ProductId) ??
                      throw new InvalidDataException($"Product {storeProduct.ProductId} does not exist");
        var oldPrice = storeProduct.SellingPrice;
        if (storeProduct.Promotional)
        {
            var nonPromotional = _storeProductRepository.GetNonPromByProm(storeProduct.UPC);
            if (nonPromotional is null) throw new InvalidDataException($"Promotional {storeProduct.UPCProm} does not exist");
            oldPrice = nonPromotional.SellingPrice;
        }
        return new(
            storeProduct.UPC,
            product.Name,
            storeProduct.SellingPrice,
            product.Characteristics,
            storeProduct.Quantity,
            storeProduct.Promotional,
            oldPrice);
    }
}