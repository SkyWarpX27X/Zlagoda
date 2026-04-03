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

    public StoreProductDBModel? GetProduct(string upc)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = "SELECT * FROM Product WHERE UPC = @upc";
        command.Parameters.AddWithValue("@upc", upc);
        using var reader = command.ExecuteReader();
        return reader.Read() ? MapStoreProduct(reader) : null;
    }
    
    // Problem: need to get all information (which probably includes name too) so I need to join and create a new entity type?
    // Also need sorting by name (which also requires joining) and an optional ternary logic parameter (null/promotional/not).
    public IEnumerable<StoreProductDBModel> GetProducts(bool sortByName = true, bool sortByQuantity = true)
    {
        using var command = _connection.CreateCommand();
        var query = "SELECT * FROM Store_Product";

        if (sortByName) query += " ORDER BY product_name";
        command.CommandText = query;
        using var reader = command.ExecuteReader();
        while (reader.Read())
            yield return MapStoreProduct(reader);
    }
}