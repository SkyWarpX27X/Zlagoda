using DTOModels;
using Services.Category;
using Services.Product;

namespace Zlagoda.ViewModels;

public class ProductsVM
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;
    public string SelectedCategory { get; set; } = "";
    public IEnumerable<ProductDTO> Products => _productService.GetProducts(string.IsNullOrWhiteSpace(SelectedCategory) ? null : SelectedCategory);
    public IEnumerable<string> Categories => _categoryService.GetCategories().Select(c => c.Name);
    public bool IsCreating { get; private set; }
    public ProductDTO? NewProduct { get; private set; }
    public string? ErrorMessage { get; private set; }
    
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    
    public ProductsVM(IProductService productService, ICategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }
    
    public int? GetSoldAmount(long productId)
    {
        if (!FromDate.HasValue || !ToDate.HasValue) return null;
        
        // TODO: call real service method to get sold amount
        // return _productService.GetSoldAmount(productId, FromDate.Value, ToDate.Value);
        return 0;
    }
    
    public void ShowCreateNew()
    {
        NewProduct = new ProductDTO();
        IsCreating = true;
        ErrorMessage = null;
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
    
    public void SaveNewProduct(ProductDTO product)
    {
        try
        {
            _productService.AddProduct(product);
            IsCreating = false;
            NewProduct = null;
            ErrorMessage = null;
        }
        catch (InvalidDataException e)
        {
            ErrorMessage = e.Message;
        }
    }

    public void EditProduct(ProductDTO product)
    {
        try
        {
            _productService.UpdateProduct(product);
        }
        catch (InvalidDataException e)
        {
            ErrorMessage = e.Message;
        }
    }

    public void DeleteProduct(long id)
    {
        _productService.DeleteProduct(id);
    }
}