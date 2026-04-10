using System.Text.RegularExpressions;
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

    public IEnumerator<StoreProductDTO> GetStoreProducts(bool sortByQuantity)
    {
        foreach (var storeProduct in _storeProductRepository.GetStoreProducts(sortByQuantity: sortByQuantity))
        {
            var product = _productRepository.GetProduct(storeProduct.ProductId);
            if (product is null) throw new InvalidDataException($"Product {storeProduct.ProductId} does not exist");
            var oldPrice = storeProduct.SellingPrice;
            if (!storeProduct.Promotional)
            {
                if (string.IsNullOrEmpty(storeProduct.UPCProm)) throw new InvalidDataException($"Promotional UPC is required");
                var promotional = _storeProductRepository.GetStoreProduct(storeProduct.UPCProm);
                if (promotional is null) throw new InvalidDataException($"Promotional {storeProduct.UPCProm} does not exist");
                oldPrice = promotional.SellingPrice;
            }
            yield return new(
                storeProduct.UPC,
                storeProduct.Name,
                storeProduct.SellingPrice,
                product.Characteristics,
                storeProduct.Quantity,
                storeProduct.Promotional,
                oldPrice);
        }
    }

    public StoreProductDTO? GetStoreProduct(string upc)
    {
        var storeProduct = _storeProductRepository.GetStoreProduct(upc);
        if (storeProduct is null) throw new InvalidDataException($"Store product {upc} doesn't exist");
        var product = _productRepository.GetProduct(storeProduct.ProductId);
        if (product is null) throw new InvalidDataException($"Product {storeProduct.ProductId} does not exist");
        var oldPrice = storeProduct.SellingPrice;
        if (!storeProduct.Promotional)
        {
            if (string.IsNullOrEmpty(storeProduct.UPCProm)) throw new InvalidDataException($"Promotional UPC is required");
            var promotional = _storeProductRepository.GetStoreProduct(storeProduct.UPCProm);
            if (promotional is null) throw new InvalidDataException($"Promotional {storeProduct.UPCProm} does not exist");
            oldPrice = promotional.SellingPrice;
        }
        return new(
            storeProduct.UPC,
            storeProduct.Name,
            storeProduct.SellingPrice,
            product.Characteristics,
            storeProduct.Quantity,
            storeProduct.Promotional,
            oldPrice);
    }

    public void AddStoreProduct(StoreProductModifyDTO storeProduct)
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
}