using DTOModels;
using Services.Product;
using Zlagoda.Test;

namespace Zlagoda.ViewModels;

public class ProductsVM
{
    //TODO switch to real product service
    //private readonly IProductService _productService;

    public IEnumerable<ProductDTO> Products { get; private set; }
    public bool IsCreating { get; private set; }
    public ProductDTO? NewProduct { get; private set; }
    
    public ProductsVM()
    {
        //_productService = productService;
        Products = new List<ProductDTO>();
    }

    public void LoadProducts()
    {
        //Products = _productService.GetProducts();
        Products = FakeProducts.GetProducts();
    }

    public void ShowCreateNew()
    {
        NewProduct = new ProductDTO();
        IsCreating = true;
    }

    public void CancelCreate()
    {
        IsCreating = false;
        NewProduct = null;
    }
    
    public void SaveNewProduct(ProductDTO product)
    {
        //_productService.AddProduct(product);
        IsCreating = false;
        NewProduct = null;
        LoadProducts();
    }

    public void EditProduct(ProductDTO product)
    {
        //_productService.UpdateProduct(product);
        LoadProducts();
    }

    public void DeleteProduct(long id)
    {
        //_productService.DeleteProduct(id);
        LoadProducts();
    }
}
