using DBModels;
using DTOModels;
using Repositories.Category;
using Repositories.Product;

namespace Services.Product;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    
    public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public IEnumerable<ProductDTO> GetProducts(string? categoryName)
    {
        foreach (var product in _productRepository.GetProducts(categoryName: categoryName))
        {
            yield return ProductDbToDto(product);
        }
    }

    public ProductDTO GetProduct(long id)
    {
        var product = _productRepository.GetProduct(id);
        if (product is null) throw new InvalidDataException($"Product {id} does not exist");
        return ProductDbToDto(product);
    }

    public int GetTotalUnits(long id, (DateTime StartDate, DateTime EndDate) dates)
    {
        var resultDates = DateRangeToStrings(dates.StartDate, dates.EndDate);
        return _productRepository.GetTotalUnits(id, resultDates);
    }

    public void AddProduct(ProductDTO product)
    {
        Validation.ValidateProduct(product);
        var category = _categoryRepository.GetCategory(product.Category)
                       ?? throw new InvalidDataException("Category does not exist");
        _productRepository.AddProduct(new(
            category.Id,
            product.Name,
            product.Characteristics,
            product.Manufacturer));
    }

    public void UpdateProduct(ProductDTO product)
    {
        Validation.ValidateProduct(product);
        var category = _categoryRepository.GetCategory(product.Category)
                       ?? throw new InvalidDataException("Category does not exist");
        _productRepository.UpdateProduct(new(
            product.Id,
            category.Id,
            product.Name,
            product.Characteristics,
            product.Manufacturer));
    }

    public void DeleteProduct(long id)
    {
        _productRepository.DeleteProduct(id);
    }

    private ProductDTO ProductDbToDto(ProductDBModel product)
    {
        var category = _categoryRepository.GetCategory(product.CategoryId);
        if (category is null) throw new InvalidDataException($"Category {product.CategoryId} does not exist");
        return new(
            product.Id,
            product.Name,
            category.Name,
            product.Characteristics,
            product.Manufacturer);
    }
    
    private (string, string) DateRangeToStrings(DateTime start, DateTime end)
    {
        var a = start.ToString("yyyy-MM-dd HH:mm:ss");
        var b = end.ToString("yyyy-MM-dd HH:mm:ss");
        return (a, b);
    }
}