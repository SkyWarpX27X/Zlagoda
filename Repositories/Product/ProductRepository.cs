using DBModels;
using Microsoft.Data.Sqlite;

namespace Repositories.Product;

public class ProductRepository : IProductRepository
{
    private readonly SqliteConnection _connection;
    public ProductRepository(SqliteConnection connection)
    {
        _connection = connection;
    }
    private static ProductDBModel MapProduct(SqliteDataReader reader) => new(
        reader.GetInt64(reader.GetOrdinal("id_product")),
        reader.GetInt64(reader.GetOrdinal("category_number")),
        reader.GetString(reader.GetOrdinal("product_name")),
        reader.GetString(reader.GetOrdinal("manufacturer")),
        reader.GetString(reader.GetOrdinal("characteristics"))
    );

    public ProductDBModel? GetProduct(long id)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = "SELECT * FROM Product WHERE id_product = @id";
        command.Parameters.AddWithValue("@id", id);
        using var reader = command.ExecuteReader();
        return reader.Read() ? MapProduct(reader) : null;
    }
    
    public IEnumerable<ProductDBModel> GetProducts(bool sortByName = true, string? categoryName = null, 
        string? productName = null)
    {
        using var command = _connection.CreateCommand();
        var query = "SELECT * FROM Product";
        if (!string.IsNullOrEmpty(categoryName))
        {
            query += " WHERE category_name = @categoryName";
            if (!string.IsNullOrEmpty(productName)) query += " AND product_name = @productName";
        }
        if (!string.IsNullOrEmpty(productName) && string.IsNullOrEmpty(categoryName))
        {
            query += " WHERE product_name = @productName";
        }
        if (sortByName) query += " ORDER BY product_name";
        command.CommandText = query;
        if (!string.IsNullOrEmpty(categoryName)) command.Parameters.AddWithValue("@categoryName", categoryName);
        if (!string.IsNullOrEmpty(productName)) command.Parameters.AddWithValue("@productName", productName);
        using var reader = command.ExecuteReader();
        while (reader.Read())
            yield return MapProduct(reader);
    }

    public void AddProduct(ProductDBModel product)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = """
                              INSERT INTO Product (category_number, product_name, characteristics, manufacturer) 
                              VALUES (@category_number, @product_name, @characteristics, @manufacturer);
                              """;
        command.Parameters.AddWithValue("@category_number", product.CategoryId);
        command.Parameters.AddWithValue("@product_name", product.Name);
        command.Parameters.AddWithValue("@characteristics", product.Characteristics);
        command.Parameters.AddWithValue("@manufacturer", product.Manufacturer);
        command.ExecuteNonQuery();
    }

    public void UpdateProduct(ProductDBModel product)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = """
                              UPDATE Product 
                              SET 
                                category_number = @category_number,
                                product_name = @product_name,
                                characteristics = @characteristics,
                                manufacturer = @manufacturer
                              WHERE
                                  id_product = @id_product;
                              """;
        command.Parameters.AddWithValue("@category_number", product.CategoryId);
        command.Parameters.AddWithValue("@product_name", product.Name);
        command.Parameters.AddWithValue("@characteristics", product.Characteristics);
        command.Parameters.AddWithValue("@manufacturer", product.Manufacturer);
        command.Parameters.AddWithValue("@id_product", product.Id);
        command.ExecuteNonQuery();
    }

    public void DeleteProduct(ProductDBModel product)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = "DELETE FROM Product WHERE id_product = @id_product;";
        command.Parameters.AddWithValue("@id_product", product.Id);
        command.ExecuteNonQuery();
    }
}