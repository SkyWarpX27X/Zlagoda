using DTOModels;
using Zlagoda.Test;

namespace Zlagoda.ViewModels;

public class ProductsVM
{
    //TODO switch to real product service
    //private readonly IProductService _productService;

    public IEnumerable<ProductDto> Products;
    
    public ProductsVM()
    {
        //_productService = productService;
        Products = new List<ProductDto>();
    }

    public void LoadProducts()
    {
        //Products = _productService.GetProducts();
        Products = FakeProducts.GetProducts();
    }
}