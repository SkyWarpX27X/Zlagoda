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

    public void AddProduct(ProductDTO product)
    {
        ValidateProduct(product, out CategoryDBModel? category);
        _productRepository.AddProduct(new(
            category!.Id,
            product.Name,
            product.Characteristics,
            product.Manufacturer));
    }

    public void UpdateProduct(ProductDTO product)
    {
        ValidateProduct(product, out CategoryDBModel? category);
        _productRepository.UpdateProduct(new(
            product.Id,
            category!.Id,
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

    private void ValidateProduct(ProductDTO product, out CategoryDBModel? category)
    {
        if (string.IsNullOrEmpty(product.Name)) throw new InvalidDataException("Name is required");
        if (string.IsNullOrEmpty(product.Category)) throw new InvalidDataException("Category is required");
        category = _categoryRepository.GetCategory(product.Category);
        if (category is null) throw new InvalidDataException($"Category {product.Category} does not exist");
        if (string.IsNullOrEmpty(product.Characteristics)) throw new InvalidDataException("Characteristics are required");
        if (string.IsNullOrEmpty(product.Manufacturer)) throw new InvalidDataException("Manufacturer is required");
    }
}