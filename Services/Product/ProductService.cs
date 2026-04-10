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
            var category = _categoryRepository.GetCategory(product.CategoryId);
            if (category is null) throw new InvalidDataException($"Category {product.CategoryId} does not exist");
            yield return new(
                product.Id,
                product.Name,
                category.Name,
                product.Characteristics,
                product.Manufacturer);
        }
    }

    public ProductDTO? GetProduct(long id)
    {
        var product = _productRepository.GetProduct(id);
        if (product is null) return null;
        var category = _categoryRepository.GetCategory(product.CategoryId);
        if (category is null) throw new InvalidDataException($"Category {product.CategoryId} does not exist");
        return new(
            product.Id,
            product.Name,
            category.Name,
            product.Characteristics,
            product.Manufacturer);
    }

    public void AddProduct(ProductDTO product)
    {
        if (string.IsNullOrEmpty(product.Name)) throw new InvalidDataException("Name is required");
        if (string.IsNullOrEmpty(product.Category)) throw new InvalidDataException("Category is required");
        var category = _categoryRepository.GetCategories(false).FirstOrDefault(
            x => x.Name == product.Category);
        if (category is null) throw new InvalidDataException($"Category {product.Category} does not exist");
        if (string.IsNullOrEmpty(product.Characteristics)) throw new InvalidDataException("Characteristics are required");
        if (string.IsNullOrEmpty(product.Manufacturer)) throw new InvalidDataException("Manufacturer is required");
        _productRepository.AddProduct(new(
            category.Id,
            product.Name,
            product.Characteristics,
            product.Manufacturer));
    }

    public void UpdateProduct(ProductDTO product)
    {
        if (string.IsNullOrEmpty(product.Name)) throw new InvalidDataException("Name is required");
        if (string.IsNullOrEmpty(product.Category)) throw new InvalidDataException("Category is required");
        var category = _categoryRepository.GetCategories(false).FirstOrDefault(
            x => x.Name == product.Category);
        if (category is null) throw new InvalidDataException($"Category {product.Category} does not exist");
        if (string.IsNullOrEmpty(product.Characteristics)) throw new InvalidDataException("Characteristics are required");
        if (string.IsNullOrEmpty(product.Manufacturer)) throw new InvalidDataException("Manufacturer is required");
        _productRepository.UpdateProduct(new(
            category.Id,
            product.Name,
            product.Characteristics,
            product.Manufacturer));
    }

    public void DeleteProduct(long id)
    {
        var product = _productRepository.GetProduct(id);
        if (product is null) return;
        _productRepository.DeleteProduct(product);
    }
}