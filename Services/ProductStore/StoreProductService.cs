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

    public IEnumerable<StoreProductDTO> GetStoreProducts(bool sortByQuantity, bool sortByName)
    {
        foreach (var storeProduct in _storeProductRepository.GetStoreProducts(sortByName, sortByQuantity))
        {
            if (storeProduct.UPCProm is null)
                yield return StoreProductDbToDto(storeProduct);
        }
    }

    public IEnumerable<StoreProductDTO> GetNonPromotionalStoreProducts(bool sortByQuantity, bool sortByName)
    {
        foreach (var storeProduct in _storeProductRepository.GetStoreProductsNonPromotional(sortByName, sortByQuantity))
        {
            if (storeProduct.UPCProm is null)
                yield return StoreProductDbToDto(storeProduct);
        }
    }

    public IEnumerable<StoreProductDTO> GetPromotionalStoreProducts(bool sortByQuantity, bool sortByName)
    {
        foreach (var storeProduct in _storeProductRepository.GetStoreProductsPromotional(sortByName, sortByQuantity))
        {
            if (storeProduct.UPCProm is null)
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
        var product = _productRepository.GetProduct(storeProduct.ProductId);
        if (product is null) throw new InvalidDataException($"Product {storeProduct.ProductId} does not exist");
        var upcProm = storeProduct.UpcProm;
        var quantity = storeProduct.Quantity;
        var existing = _storeProductRepository.GetStoreProductsByProductId(storeProduct.ProductId);
        if (existing.nonProm is not null)
        {
            upcProm = existing.nonProm.UPCProm;
            if (existing.prom is not null)
            {
                _storeProductRepository.UpdateStoreProduct(new(
                    existing.prom.UPC,
                    existing.prom.UPCProm,
                    existing.prom.ProductId,
                    existing.prom.SellingPrice,
                    existing.prom.Quantity + quantity,
                    existing.prom.Promotional
                    ));
                quantity = 0;
            }
            else
            {
                quantity += existing.nonProm.Quantity;
            }
            _storeProductRepository.DeleteStoreProduct(existing.nonProm.UPC);
        }
        
        _storeProductRepository.AddStoreProduct(new(
            storeProduct.Upc,
            upcProm,
            storeProduct.ProductId,
            storeProduct.Price,
            quantity,
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

    public void AddPromotionalStoreProduct(string originalUpc, string promotionalUpc)
    {
        if (!Regex.IsMatch(promotionalUpc, @"\d{12}"))
            throw new InvalidDataException("Invalid promotional UPC");

        {
            var promotional = _storeProductRepository.GetStoreProduct(promotionalUpc);
            if (promotional is not null) throw new InvalidDataException($"Store product {promotionalUpc} already exists.");
        }
        
        var original = _storeProductRepository.GetStoreProduct(originalUpc);
        if (original is null) throw new InvalidDataException($"Store product {originalUpc} does not exist.");
        if (original.Promotional) throw new InvalidDataException($"Store product {originalUpc} is promotional.");
        if (original.UPCProm is not null) throw new InvalidDataException($"Store product {originalUpc} already has a promotional product.");
        
        _storeProductRepository.AddStoreProduct(new(
            promotionalUpc,
            null,
            original.ProductId,
            original.SellingPrice * 0.8m,
            original.Quantity,
            true
            ));
        _storeProductRepository.UpdateStoreProduct(new(
            original.UPC,
            promotionalUpc,
            original.ProductId,
            original.SellingPrice,
            0,
            original.Promotional
        ));
    }

    public void DeletePromotionalStoreProduct(string promotionalUpc)
    {
        var promotional = _storeProductRepository.GetStoreProduct(promotionalUpc);
        if (promotional is null) throw new InvalidDataException($"Promotional store product {promotionalUpc} doesn't exist.");
        
        var original = _storeProductRepository.GetNonPromByProm(promotionalUpc);
        if (original is not null)
        {
            _storeProductRepository.UpdateStoreProduct(new(
                original.UPC,
                null,
                original.ProductId,
                original.SellingPrice,
                promotional.Quantity,
                original.Promotional
            ));
        }
        _storeProductRepository.DeleteStoreProduct(promotionalUpc);
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
    }
}