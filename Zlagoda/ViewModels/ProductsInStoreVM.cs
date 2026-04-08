using DTOModels;
using Zlagoda.Test;

namespace Zlagoda.ViewModels;

public class ProductsInStoreVM
{
    public IEnumerable<StoreProductDTO> ProductsInStore { get; private set; }
    
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
    
    public ProductsInStoreVM()
    {
        ProductsInStore = new List<StoreProductDTO>();
        AvailableProducts = new List<ProductDTO>();
    }

    public void LoadProducts()
    {
        // TODO: Load from service
        ProductsInStore = FakeStoreProducts.GetProducts();
        
        // TODO: Load from Product service
        AvailableProducts = FakeProducts.GetProducts();
    }

    public void ShowCreateNew()
    {
        NewProduct = new StoreProductModifyDTO("", 0, 0);
        IsCreating = true;
    }

    public void SaveNewProduct(StoreProductModifyDTO product)
    {
        IsCreating = false;
        NewProduct = null;
        // TODO: save product via service
        LoadProducts();
    }

    public void CancelCreate()
    {
        IsCreating = false;
        NewProduct = null;
    }

    public void EditProduct(StoreProductModifyDTO product)
    {
        // TODO: edit product via service
        LoadProducts();
    }

    public void DeleteProduct(string upc)
    {
        // TODO: delete product via service
        LoadProducts();
    }
    
    public void MakeProm(string upc)
    {
        // TODO: call make prom service method
        LoadProducts();
    }

    public void CancelProm(string upc)
    {
        // TODO: call cancel prom service method
        LoadProducts();
    }
}
