using DBModels;
using Microsoft.Data.Sqlite;

namespace Repositories.StoreProduct;

public class StoreProductRepository
{
    private readonly SqliteConnection _connection;
    public StoreProductRepository(SqliteConnection connection)
    {
        _connection = connection;
    }
    private static StoreProductDBModel MapStoreProduct(SqliteDataReader reader) => new(
        reader.GetString(reader.GetOrdinal("UPC")),
        reader.GetString(reader.GetOrdinal("UPC_prom")),
        reader.GetInt64(reader.GetOrdinal("id_product")),
        reader.GetDecimal(reader.GetOrdinal("selling_price")),
        reader.GetInt32(reader.GetOrdinal("products_number")),
        reader.GetBoolean(reader.GetOrdinal("promotional_product"))
    );

    public StoreProductDBModel? GetStoreProduct(string upc)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = "SELECT * FROM Product WHERE UPC = @upc";
        command.Parameters.AddWithValue("@upc", upc);
        using var reader = command.ExecuteReader();
        return reader.Read() ? MapStoreProduct(reader) : null;
    }
    
    public IEnumerable<StoreProductDBModel> GetStoreProducts(bool sortByName = true, bool sortByQuantity = false)
    {
        using var command = _connection.CreateCommand();
        // Since we join, it technically returns all attributes. Is that necessarily bad? I'm not sure, since I think
        // SELECT basically just does a projection on the final result anyway, and we just ignore that by mapping it to
        // the DB model.
        var query = "SELECT * FROM Store_Product";
        if (sortByName || sortByQuantity) 
            query += $"""
                     JOIN Product ON Store_Product.id_product = Product.id_product
                     ORDER BY {(sortByName ? "product_name" : "products_number")}
                     """;
        command.CommandText = query;
        using var reader = command.ExecuteReader();
        while (reader.Read())
            yield return MapStoreProduct(reader);
    }

    public IEnumerable<StoreProductDBModel> GetStoreProductsPromotional(bool sortByName = true, bool sortByQuantity = false)
    {
        using var command = _connection.CreateCommand();
        var query = "SELECT * FROM Store_Product";
        if (sortByName || sortByQuantity) 
            query += $"""
                      JOIN Product ON Store_Product.id_product = Product.id_product
                      WHERE promotional_product IS TRUE
                      ORDER BY {(sortByName ? "product_name" : "products_number")}
                      """;
        command.CommandText = query;
        using var reader = command.ExecuteReader();
        while (reader.Read())
            yield return MapStoreProduct(reader);
    }
    
    public IEnumerable<StoreProductDBModel> GetStoreProductsNonPromotional(bool sortByName = true, bool sortByQuantity = false)
    {
        using var command = _connection.CreateCommand();
        var query = "SELECT * FROM Store_Product";
        if (sortByName || sortByQuantity) 
            query += $"""
                      JOIN Product ON Store_Product.id_product = Product.id_product
                      WHERE promotional_product IS FALSE
                      ORDER BY {(sortByName ? "product_name" : "products_number")}
                      """;
        command.CommandText = query;
        using var reader = command.ExecuteReader();
        while (reader.Read())
            yield return MapStoreProduct(reader);
    }
    
    // IDK if I should add Manager query 21 here or not it feels very general help
}