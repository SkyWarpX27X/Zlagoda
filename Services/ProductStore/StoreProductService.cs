using System.Text.RegularExpressions;
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

    public IEnumerable<StoreProductDTO> GetStoreProducts(bool sortByQuantity)
    {
        foreach (var storeProduct in _storeProductRepository.GetStoreProducts(sortByQuantity: sortByQuantity))
        {
            yield return StoreProductDbToDto(storeProduct);
        }
    }

    public StoreProductDTO GetStoreProduct(string upc)
    {
        var storeProduct = _storeProductRepository.GetStoreProduct(upc);
        if (storeProduct is null) throw new InvalidDataException($"Product {upc} does not exist");
        return StoreProductDbToDto(storeProduct);
    }

    public void AddStoreProduct(StoreProductModifyDTO storeProduct)
    {
        ValidateStoreProduct(storeProduct);
        
        _storeProductRepository.AddStoreProduct(new(
            storeProduct.Upc,
            storeProduct.UpcProm,
            storeProduct.ProductId,
            storeProduct.Price,
            storeProduct.Quantity,
            storeProduct.Promotional));
    }

    public void UpdateStoreProduct(StoreProductModifyDTO storeProduct)
    {
        ValidateStoreProduct(storeProduct);
        
        _storeProductRepository.UpdateStoreProduct(new(
            storeProduct.Upc,
            storeProduct.UpcProm,
            storeProduct.ProductId,
            storeProduct.Price,
            storeProduct.Quantity,
            storeProduct.Promotional));
    }

    public void DeleteStoreProduct(string upc)
    {
        _storeProductRepository.DeleteStoreProduct(upc);
    }

    private StoreProductDTO StoreProductDbToDto(StoreProductInfoDataModel storeProduct)
    {
        var product = _productRepository.GetProduct(storeProduct.ProductId);
        if (product is null) throw new InvalidDataException($"Product {storeProduct.ProductId} does not exist");
        var oldPrice = storeProduct.SellingPrice;
        if (!string.IsNullOrEmpty(storeProduct.UPCProm))
        {
            var promotional = _storeProductRepository.GetStoreProduct(storeProduct.UPCProm);
            if (promotional is null) throw new InvalidDataException($"Promotional {storeProduct.UPCProm} does not exist");
            oldPrice = promotional.SellingPrice;
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

    private void ValidateStoreProduct(StoreProductModifyDTO storeProduct)
    {
        if (string.IsNullOrEmpty(storeProduct.Upc))
            throw new InvalidDataException("UPC is required");
        if (!Regex.IsMatch(storeProduct.Upc, @"\d{12}"))
            throw new InvalidDataException("Invalid UPC");
        if (storeProduct.Quantity < 0)
            throw new InvalidDataException("Quantity cannot be negative");
        if (storeProduct.Price < 0)
            throw new InvalidDataException("Price cannot be negative");
        if (!string.IsNullOrEmpty(storeProduct.UpcProm) && !Regex.IsMatch(storeProduct.UpcProm, @"\d{12}"))
            throw new InvalidDataException("Invalid promotion UPC");
        var product = _productRepository.GetProduct(storeProduct.ProductId);
        if (product is null) throw new InvalidDataException($"Product {storeProduct.ProductId} does not exist");
    }
}