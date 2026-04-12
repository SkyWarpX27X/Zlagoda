using DTOModels;
using Services.Product;
using Services.ProductStore;

namespace Zlagoda.ViewModels;

public class ProductsInStoreVM
{
    private readonly IStoreProductService _storeProductService;
    private readonly IProductService _productService;
    public IEnumerable<StoreProductDTO> ProductsInStore { get; private set; }
    public string? ErrorMessage { get; private set; }
    
    //TODO replace with service
    public IEnumerable<StoreProductDTO> FilteredAndSortedProducts
    {
        get
        {
            var query = ProductsInStore.AsQueryable();

            // Search
            if (!string.IsNullOrWhiteSpace(SearchUpc))
            {
                query = query.Where(p => p.Upc == SearchUpc);
            }

            // Filter
            if (PromFilter == "PromOnly")
            {
                query = query.Where(p => p.IsProm);
            }
            else if (PromFilter == "NonPromOnly")
            {
                query = query.Where(p => !p.IsProm);
            }

            // Sort
            return SortBy switch
            {
                "Name" => query.OrderBy(p => p.Name).ToList(),
                "Quantity" => query.OrderBy(p => p.Quantity).ToList(),
            };
        }
    }
    
    public IEnumerable<ProductDTO> AvailableProducts { get; private set; }
    
    public bool IsCreating { get; private set; }
    public StoreProductModifyDTO? NewProduct { get; private set; }
    
    public string SearchUpc { get; set; } = "";
    public string PromFilter { get; set; } = "All";
    public string SortBy { get; set; } = "Name";
    
    public ProductsInStoreVM(IStoreProductService storeProductService, IProductService productService)
    {
        _storeProductService = storeProductService;
        _productService = productService;
        ProductsInStore = new List<StoreProductDTO>();
        AvailableProducts = new List<ProductDTO>();
    }

    public void LoadProducts()
    {
        ProductsInStore = _storeProductService.GetStoreProducts();
        AvailableProducts = _productService.GetProducts();
    }

    public void ShowCreateNew()
    {
        NewProduct = new StoreProductModifyDTO("", 0, 0);
        IsCreating = true;
        ErrorMessage = null;
    }

    public void SaveNewProduct(StoreProductModifyDTO product)
    {
        try
        {
            _storeProductService.AddStoreProduct(product);
            IsCreating = false;
            NewProduct = null;
            ErrorMessage = null;
        }
        catch (InvalidDataException e)
        {
            ErrorMessage = e.Message;
        }
        LoadProducts();
    }

    public void CancelCreate()
    {
        IsCreating = false;
        NewProduct = null;
        ErrorMessage = null;
    }

    public void ClearError()
    {
        ErrorMessage = null;
    }

    public void EditProduct(StoreProductModifyDTO product)
    {
        _storeProductService.UpdateStoreProduct(product);
        LoadProducts();
    }

    public void DeleteProduct(string upc)
    {
        _storeProductService.DeleteStoreProduct(upc);
        LoadProducts();
    }
    
    public void MakeProm((string originalUpc, string promUpc) data)
    {
        // TODO: call make prom service method using data.originalUpc and data.promUpc
        LoadProducts();
    }

    public void CancelProm(string promUpc)
    {
        // TODO: call cancel prom service method
        LoadProducts();
    }
}